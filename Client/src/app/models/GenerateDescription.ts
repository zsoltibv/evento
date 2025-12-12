export interface GenerateDescriptionRequest {
  prompt: string;
  max_tokens?: number;
}

export interface GenerateDescriptionResponse {
  response: string;
}
