# Web.Mapper Toolbox

Mapping functionality using AutoMapper for the Digipolis Web toolbox

## Table of Contents

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->

- [Installation](#installation)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## DISCLAIMER

This is still a beta package and is currently being developed.
Use at your own risk.

Testing and documentation is absent/minimal but will be expanded in the future.

## Installation

To add the toolbox to a project, you add the package to the project.json :

``` json 
"dependencies": {
    "Digipolis.Web.Mapper":  "1.0.0"
 }
``` 

In Visual Studio you can also use the NuGet Package Manager to do this.

## Configuration

If you use the MapBody attribute somewhere in your project and would like to update the Api Description (e.g. for your Swagger definition), add the following to your Startup:

``` csharp
public virtual void ConfigureServices(IServiceCollection services)
{
    services.Configure<DescriptionProviderOptions<MapBodyApiDescriptionProvider>>(options => options.Order = 1);
}
```

You can change the order if you like.
However, make sure it's as low as possible.
If you have other Api Description Providers that change the casing of body or result parameters, make sure the have a greater order.