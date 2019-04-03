# ExpenseApi

API that exposes an endpoint to import data from text received via email.

## Endpoints


1. `GET /api/expense`
Retrieves a list of expense claims that have been added to an in-memory collection.

1. `POST /api/expense`
Adds an expense to the in-memory collection

1. `PUT /api/expense`
Idempotent endpoint that returns an Expense object parsed from a string
