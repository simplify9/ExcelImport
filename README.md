| **Package**       | **Version** |
| :----------------:|:----------------------:|
| [`SimplyWorks.ExcelImport`](https://www.nuget.org/packages/SimplyWorks.ExcelImport/)|![Nuget](https://img.shields.io/nuget/v/SimplyWorks.ExcelImport?style=for-the-badge)|

![License](https://img.shields.io/badge/license-MIT-blue.svg)

## Intro

*ExcelImport* is a library that provides importing methods to overload.
The main function is checking out `ExcelService` with its import method. It takes in a URL and some other options. 
*ExcelImport* has a loader. We use it check in services. The way to do that is it takes in a URL, passes it to the reader (another service), and reads it. These are all services provided by *ExcelImport*, which means the user can use the services either individually or use the big `ExcelService` to import them, which includes all underlying services.

The `reader`: takes in a file URL, some methods, and overloads of the same methods.

URL in question is the URL of a local file.

All that the `ExcelService` does is import the local file and how to parse this into a json and then it just reaches the individual rows. It also has an excel repo, which is an abstraction in the dbContext, a way into the database.

## Technical stuff

The options: the user needs to specify the naming stategy (camelCase, CapitalCase, snake_case).


## Getting support 👷

If you encounter any bugs, don't hesitate to submit an [issue](https://github.com/simplify9/ExcelImport/issues). We'll get back to you promptly!
