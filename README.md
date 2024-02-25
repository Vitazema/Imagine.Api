# AI-Generated Images API Service (ASP.NET)

## Overview

This ASP.NET-based API service offers fast, reactive AI-generated images, utilizing the power of Stable Diffusion models. Enhanced with caching, our service ensures efficient performance and quick response times, catering to diverse image generation needs.

## Features

- **AI-Generated Images:** Utilizes Stable Diffusion models to produce high-quality, AI-generated images.
- **Fast and Reactive:** Engineered for speed, our service quickly responds to requests, providing a seamless experience.
- **Cache-Injected:** Implements caching strategies to enhance performance and reduce response times.
- **ASP.NET Core Framework:** Built on the robust ASP.NET Core framework, ensuring scalability and security.

## Getting Started

### Prerequisites

- .NET 5.0 SDK or later
- Visual Studio 2019 or later (recommended for development)

### Installation

1. Clone the repository:
git clone https://github.com/Vitazema/Imagine.Api.git

2. Open the solution in Visual Studio.
3. Restore NuGet packages:
Right-click on the solution > Restore NuGet Packages
4. Build the solution:
Build > Build Solution

### Running the Service

- In Visual Studio, set the API project as the startup project.
- Press `F5` to start debugging. The API service will start on localhost with a port number assigned by Visual Studio.

### Making Requests

To generate an image, send a POST request to `/arts` with a JSON payload specifying the image details:
```json
{
"prompt": "Describe the image you want to generate",
"parameters": {...}
}
```

The API will process the request and return a link to the generated image.

### Contributing
We welcome contributions! If you have suggestions for improvements or encounter any issues, please feel free to submit a pull request or open an issue.

### License
This project is licensed under the MIT License. See the LICENSE file for more details.
