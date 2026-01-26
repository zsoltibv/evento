FROM python:3.11-slim

WORKDIR /app

COPY PyCharmMiscProject/requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

COPY PyCharmMiscProject/ .

EXPOSE 8000

CMD ["uvicorn", "script:app", "--host", "0.0.0.0", "--port", "8000", "--reload"]
