# ---------- Build stage ----------
FROM node:20-alpine AS build
WORKDIR /app

# Install deps
COPY package*.json ./
RUN npm install

# Copy source
COPY . .

# Build Angular (uses default = production)
RUN npm run build

# ---------- Runtime stage ----------
FROM nginx:alpine

# Angular 17+ output path
COPY --from=build /app/dist/Client/browser /usr/share/nginx/html

# Optional: remove default nginx config noise
RUN rm /etc/nginx/conf.d/default.conf

# Minimal nginx config
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
