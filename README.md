# POS For Small Shop

## Overview
A Windows application designed for small coffee shops with single-staff operations. Staff can open shifts, place orders, and process payments via cash or QR code. Features include receipt management, customer tracking, and shift reporting.

## Features
- **Shift Management**: Open, close, and track staff shifts
- **Order Processing**: Create and manage customer orders
- **Payment Handling**: Accept payments via cash or QR code
- **Receipt Management**: Generate and print customer receipts
- **Customer Tracking**: Build customer database for loyalty programs
- **Reporting**: Generate end-of-shift and sales reports

## Tech Stack
- **Frontend**: WinUI 3
- **Architecture**: MVVM (Model-View-ViewModel)
- **Design Patterns**: IRepository, Singleton, DAO (Data Access Object)
- **Backend**: GraphQL API via Graphile
- **Database**: PostgreSQL
- **Containerization**: Docker

## Project Structure
- **POS For Small Shop**: Main WinUI 3 application code
- **API**: GraphQL API server powered by Graphile and PostgreSQL

## Setup Instructions

### API Setup (Docker)
1. Navigate to the `api` directory:
   ```
   cd api
   ```

2. Build and start the Docker containers, recommend using manually build description in /API/README.md:
   ```
   docker-compose up -d
   ```

3. The GraphQL API will be available at: http://localhost:5000/graphql

### WinUI 3 Application Setup
1. Open the solution in Visual Studio 2022
2. Make sure you have the Windows App SDK installed
3. Build and run the application

## Development
- The application follows MVVM architecture
- Data access is implemented using the Repository pattern
- API communication is handled through GraphQL

## Requirements
- Windows 10 version 1809 or later
- Visual Studio 2022 with Windows App SDK workload
- Docker Desktop (for running the API)

