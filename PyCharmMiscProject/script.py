from fastapi import FastAPI
from pydantic import BaseModel
from transformers import AutoTokenizer, AutoModelForCausalLM
import torch

app = FastAPI()

model_name = "microsoft/Phi-3-mini-4k-instruct"

print("Loading model...")
tokenizer = AutoTokenizer.from_pretrained(model_name)
model = AutoModelForCausalLM.from_pretrained(
    model_name,
    device_map="cpu",           # CPU mode
    torch_dtype=torch.float16   # saves RAM
)
device = next(model.parameters()).device
print("Model loaded! Server is ready at http://127.0.0.1:8000")

class PromptRequest(BaseModel):
    prompt: str
    max_tokens: int = 128   # safe default for CPU

@app.post("/generate")
def generate_text(req: PromptRequest):
    # Make sure max_tokens is reasonable
    max_tokens = min(req.max_tokens, 256)

    # Create chat template
    messages = [
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": req.prompt}
    ]

    text = tokenizer.apply_chat_template(
        messages,
        tokenize=False,
        add_generation_prompt=True
    )

    # Encode inputs
    model_inputs = tokenizer([text], return_tensors="pt").to(device)

    # Generate with EOS token to prevent endless loop
    output = model.generate(
        **model_inputs,
        max_new_tokens=max_tokens,
        temperature=0.7,
        do_sample=True,
        eos_token_id=tokenizer.eos_token_id
    )

    # Remove prompt tokens
    generated_ids = output[0][model_inputs.input_ids.shape[1]:]
    response = tokenizer.decode(generated_ids, skip_special_tokens=True)

    return {"response": response}
