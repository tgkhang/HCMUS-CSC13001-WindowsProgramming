FROM node:18-alpine

WORKDIR /app

# Copy package files and install dependencies
COPY package*.json ./
RUN npm install

# Copy all project files (except those in .dockerignore)
COPY . .

# Wait for the database to be ready, then run migrations and seeds
CMD sh -c "echo 'Waiting for PostgreSQL to start...' && \
    sleep 10 && \
    mkdir -p migrations && \
    npx knex migrate:latest && \
    npx knex seed:run && \
    echo 'Database setup complete!'"