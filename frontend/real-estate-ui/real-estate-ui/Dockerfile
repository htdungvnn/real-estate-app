# Step 1: Build the Angular app
FROM node:18 AS build
WORKDIR /app

# Copy package.json and package-lock.json
COPY package.json package-lock.json ./

# Install dependencies
RUN npm install

# Copy the source code
COPY . .

# Build the Angular app
RUN npm run build --prod

# Step 2: Serve the app with Nginx
FROM nginx:alpine AS runtime
COPY --from=build /app/dist/real-estate-ui /usr/share/nginx/html

# Expose port 80 for the Angular app
EXPOSE 80

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]
