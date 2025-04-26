# Coffee Shop Database

This project provides a complete PostgreSQL database with tables, relationships, and sample data for a coffee shop management system.

## Quick Start

1. Make sure Docker and Docker Compose are installed on your system. If not:
   - Install [Docker](https://docs.docker.com/get-docker/)
   - Install [Docker Compose](https://docs.docker.com/compose/install/)

2. Clone or unzip this project into a directory

2. Start a PostgreSQL container:
   ```
   docker run -d --name coffee_shop_db -e POSTGRES_PASSWORD=1234 -p 5432:5432 postgres
   ```

3. Create the database:
   ```
   docker exec -it coffee_shop_db psql -U postgres -c "CREATE DATABASE database1;"
   ```

4. Install dependencies:
   ```
   npm install
   ```

5. Run migrations:
   ```
   npx knex migrate:latest
   ```

6. Run seeds:
   ```
   npx knex seed:run
   ```

7. Run API :
   ```
   postgraphile -c "postgres://postgres:1234@localhost:5432/database1" --enhance-graphiql --dynamic-json
   ```

## Database Connection Details

- **Host**: localhost
- **Port**: 5432
- **Database**: database1
- **Username**: postgres
- **Password**: 1234

You can use these details to connect to the database from any PostgreSQL client (like pgAdmin, DBeaver, etc.)

## Database Schema

The database includes the following tables:

1. **Categories**: Product categories (coffee, tea, pastries, etc.)
2. **Ingredients**: Stock of ingredients used in menu items
3. **MenuItems**: Available products for sale
4. **Orders**: Customer orders
5. **OrdersDetails**: Line items in each order
6. **Customer**: Registered customer information
7. **Transactions**: Payment transactions
8. **Promotions**: Special offers and discounts
9. **Notifications**: System notifications
10. **Shifts**: Work shifts for accounting
11. **ShiftOrders**: Orders made during specific shifts
12. **CashFlow**: Cash movements during shifts

## Project Structure

- `migrations/`: Database table definitions
- `seeds/`: Sample data to populate tables
- `knexfile.js`: Knex.js configuration
- `docker-compose.yml`: Docker configuration for easy setup