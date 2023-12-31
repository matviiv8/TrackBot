# TrackBot

TrackBot(test task) is a project that provides an API for tracking walking activities and generating insights based on the collected data.

## Table of Contents

- [Description](#description)
- [Features](#features)
- [Screenshots](#screenshots)
- [Technology Used](#technology-used)
- [License](#license)

## Description

TrackBot is designed to track walking activities and perform calculations on the collected data. It allows users to input their walking data and retrieve information such as distance covered, duration, and daily activity summaries.

## Features

- Data splitting: Splitting the walking data into separate walks based on time gaps.
- Distance calculation: Calculating the distance covered for each walk.
- Duration calculation: Calculating the duration of each walk.
- Daily activity summary: Providing a summary of the total distance covered and total duration for each day.
- Localization: Supporting localization in two languages (Ukrainian and English).
- Telegram bot integration: Integrating with a Telegram bot to provide information and insights about walks based on user input.

## Screenshots

![1](https://github.com/matviiv8/TrackBot/assets/73823120/26e81016-477c-4bb0-b6da-ef8a1cde018b)
![2](https://github.com/matviiv8/TrackBot/assets/73823120/16572460-dbee-4125-9f92-44d9a5718454)
![3](https://github.com/matviiv8/TrackBot/assets/73823120/99dcffe8-b02c-4c7d-8a16-9dee2ddbc7a2)

## Technology Used

TrackBot utilizes the following technologies:

- Programming Language: C#
- Framework: ASP.NET Core
- Database: Entity Framework Core
- Telegram Bot Integration: Telegram.Bot NuGet package
- Tunneling: ngrok

The project is developed using C# as the primary programming language and utilizes the ASP.NET Core framework for building the API. Entity Framework Core is used for managing and interacting with the database.

The Telegram.Bot NuGet package is integrated to enable seamless integration with the Telegram bot for providing insights and information about walks.

In addition, ngrok is utilized for tunneling purposes. It allows you to expose your locally hosted API to the internet, making it accessible for testing and integration with external services like the Telegram bot.

These technologies were chosen to provide a robust, secure, and accessible solution for tracking walking activities and generating insights based on the collected data.

## License

TrackBot is released under the [MIT License](LICENSE).
