# JSONPlaceholder API Data Retrieval and Storage
This .NET Core console application is designed to retrieve data from the JSONPlaceholder API and store it in a local database using Entity Framework Core. The application retrieves posts, associated comments, and users from the API and organizes the data in separate database tables. Additionally, it provides SQL queries and stored procedures to interact with the stored data.

## Table of Contents
- Prerequisites
- Getting Started
- Application Overview
- SQL Files

## Prerequisites
Before running the application, ensure that you have the following prerequisites installed:
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.Sqlite (SQLite is used as the database provider in this example)
- Newtonsoft.Json
You can use install the following using 'dotnet add package' to your project directory.

## Getting Started
You will need to create migrations for database before running your program. You can refer codes to execute in terminal here:
- dotnet ef migrations add CreateTables
- dotnet ef database update
- dotnet run

## Application Overview
Data Models:
- User: Represents user data, including name, username, email, phone, website, and company details.
- Company: A nested class within User representing company information.
- Post: Represents post data, including user ID, title, and body.
- Comment: Represents comment data, including post ID, name, email, body, and created date-time.

DbContext: AppDbContext is a database context class that uses Entity Framework Core to manage database operations. It defines the structure of the database and handles migrations.

Data Retrieval: The application fetches data from the JSONPlaceholder API for posts, comments, and users using HttpClient.

Database Schema:
- Users: Stores user information.
- Posts: Stores post information.
- Comments: Stores comment information.

## SQL Files
- sqlscript1.sql (query that retrieves all posts and their associated users, sorted by post creation date in descending order)
- sqlscript2.sql (stored procedure that takes a post ID as input and returns the post title and body associated with that post. If the post ID does not exist, the stored procedure should return an error message)
- sqlscript3.sql (a trigger that automatically inserts a new row into the “Posts” table whenever a new post is created through an API call. The trigger should set the “CreatedDateTime” column to the current date and time)
- sqlscript4.sql (query that returns the top 5 users with the most posts, along with the total number of posts for each user)






