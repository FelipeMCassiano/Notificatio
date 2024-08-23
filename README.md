# Notificatio

Notificatio is a service composed of two microservices that communicate via RabbitMQ to send emails. The microservices are configured using TOML files.

## Technologies

- C# .NET
- RabbitMQ
- MySQL

## Installation

To install and run the project, follow these steps:

1. Clone the repository:

    ```bash
    git clone https://github.com/your-username/notificatio.git
    ```

2. Navigate to the project directory:

    ```bash
    cd notificatio
    ```

3. Start the services using Docker Compose:

    ```bash
    docker-compose up -d
    ```

This will automatically start the microservices and set up RabbitMQ and MySQL.

## Configuration

Notificatio's microservices are configured using TOML files. Make sure to adjust the configuration files as needed for your environment.

## Usage

Once the services are running, they will be ready to send emails according to the implemented logic.

## Contribution

If you would like to contribute to Notificatio, please follow the guidelines below:

1. Fork the repository.
2. Create a new branch for your feature or bug fix.
3. Submit a pull request detailing your changes.

## License

This project is licensed under the [MIT License](LICENSE).


