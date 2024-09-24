## **Users API Endpoints**

### 1. **Create a User**

- **Endpoint**: `POST /api/v1/user`
- **Description**: Register a new user
- **Authentication**: No
- **Request Body**:

  ```json
  {
    "user_name": "johndoe",
    "email": "john@doe.com",
    "password": "se2345r",
    "phone_number": "123456",
    "role": "user",
    "user_address": {
      "is_default": true,
      "address": {
        "unit_number": "sdun",
        "street_number": 23,
        "address_line1": "wall street",
        "address_line2": "Marlin street",
        "city": "Helsinki",
        "postal_code": "00100",
        "country": "Finland"
      }
    }
  }
  ```

- **Response**:
  - **201 created**:
    ```json
    {
      "user_id": 23456,
      "email": "john@doe.com",
      "user_name": "johndoe",
      "phone_number": "123456",
      "role": "user",
      "user_address": {
        "is_default": true,
        "address": {
          "unit_number": "sdun",
          "street_number": 23,
          "address_line1": "wall street",
          "address_line2": "Marlin street",
          "city": "Helsinki",
          "postal_code": "00100",
          "country": "Finland"
        }
      }
    }
    ```
  - **400 Bad Request**: Invalid input data

### 2. **Get All Users**

- **Endpoint**: `GET /api/v1/users`
- **Description**: Retrieves a list of all users.
- **Authorization**: yes
- **Query Parameters**:
  - `?role={admin}` : filter by admin role
  - `?role={user}` : filter by user role
  - `?limit={number}` : Limit the number of result
- **Response**:
  - **200 OK**:
    ```json
    [
        {
            "user_id": 23456,
            "email": "john@doe.com",
            "user_name": "johndoe",
            "phone_number": "123456",
            "role": "user",
            "user_address": {
              "is_default": true,
              "address":
                {
                  "unit_number": "sdun",
                  "street_number": 23,
                  "address_line1": "wall street",
                  "address_line2": "Marlin street",
                  "city": "Helsinki",
                  "postal_code": "00100",
                  "country": "Finland"
                }
    }
        },
        ...
    ]
    ```

### 3. **Get a Single User by ID**

- **Endpoint**: `GET /api/v1/users/{userId}`
- **Description**: Retrieves details of a specific user.
- **Authentication**: no
- **Response**:

  - **200 OK**:

  ```json
  {
    "user_id": 23456,
    "user_name": "johndoe",
    "email": "john@doe.com",
    "phone_number": "123456",
    "role": "user",
    "user_address": {
      "is_default": true,
      "address": {
        "unit_number": "sdun",
        "street_number": 23,
        "address_line1": "wall street",
        "address_line2": "Marlin street",
        "city": "Helsinki",
        "postal_code": "00100",
        "country": "Finland"
      }
    }
  }
  ```

  - **404 Not Found**: User not found.

### 4. **Update a User**

- **Endpoint**: `PUT /api/v1/users/{userId}`
- **Description**: Updates the details of an existing user.
- **Authentication**: yes
- **Request Body**:
  ```json
  {
    "user_name": "Updated John Doe",
    "email": "updatedjohndoe@examp"
  }
  ```
- **Response**:
  - **200 OK**:
    ```json
    {
      "user_id": 23456,
      "user_name": "Updated John Doe",
      "email": "updatedjohndoe@examp",
      "phone_number": "123456",
      "role": "user",
      "user_address": {
        "is_default": true,
        "address": {
          "unit_number": "sdun",
          "street_number": 23,
          "address_line1": "wall street",
          "address_line2": "Marlin street",
          "city": "Helsinki",
          "postal_code": "00100",
          "country": "Finland"
        }
      }
    }
    ```
  - **404 Not Found**: User not found.

## Hard Delete User

- **Endpoint**: `DELETE /api/v1/users/{user_id}`
- **Description**: Permanently deletes a user and associated records.
- **Responses**:
  - **204 No Content**: Successfully deleted.
  - **404 Not Found**: User not found.

## Soft Delete User (optional)

- **Endpoint**: `DELETE /api/v1/users/{user_id}`
- **Description**: Marks a user as deleted without removing the record.
- **Responses**:
  - **204 No Content**: Successfully marked as deleted.
  - **404 Not Found**: User not found.

---
