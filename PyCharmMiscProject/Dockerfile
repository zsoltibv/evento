FROM python:3.11-slim AS builder

WORKDIR /app

# Install dependencies needed to build some wheels
RUN apt-get update && apt-get install -y build-essential git curl && rm -rf /var/lib/apt/lists/*

COPY requirements.txt .

# Build wheels once
RUN pip wheel --no-cache-dir --wheel-dir /wheels -r requirements.txt

# Final lightweight image
FROM python:3.11-slim

WORKDIR /app

# Copy wheels and install
COPY --from=builder /wheels /wheels
COPY requirements.txt .
RUN pip install --no-index --find-links=/wheels -r requirements.txt

# Copy app
COPY app/ ./app

EXPOSE 8000
CMD ["uvicorn", "app.script:app", "--host", "0.0.0.0", "--port", "8000"]
