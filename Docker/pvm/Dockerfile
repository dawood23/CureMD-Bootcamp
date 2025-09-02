# Stage 1: Build Angular app
FROM node:20-alpine AS build
WORKDIR /app

# Copy package files and install dependencies (include dev deps for build)
COPY package*.json ./
RUN npm ci

# Copy source code and build
COPY . .
RUN npm run build

# Stage 2: Serve with nginx
FROM nginx:alpine AS runtime

# Copy Angular build output to nginx html folder
COPY --from=build /app/dist/pvm /usr/share/nginx/html

# Copy custom nginx configuration (optional, e.g. for SPA routing)
COPY nginx.conf /etc/nginx/nginx.conf

# Create a non-root user
RUN addgroup -g 1001 -S nginx-group && \
    adduser -S -D -H -u 1001 -h /var/cache/nginx -s /sbin/nologin -G nginx-group -g nginx-group nginx-user

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
    CMD wget --quiet --tries=1 --spider http://localhost:80/ || exit 1

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
