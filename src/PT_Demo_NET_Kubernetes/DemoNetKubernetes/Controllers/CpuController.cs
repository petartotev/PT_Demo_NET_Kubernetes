﻿using Microsoft.AspNetCore.Mvc;
using Prometheus;

namespace DemoNetKubernetes.Controllers;

[ApiController]
[Route("[controller]")]
public class CpuController : ControllerBase
{
    private readonly ILogger<CpuController> _logger;

    public CpuController(ILogger<CpuController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "DoLoadOnCpu")]
    [Route("doload/{number}")]
    public ObjectResult Get(int number)
    {
        CalculatePrimes(number);

        var counter = Metrics.CreateCounter(
            "cpu_endpoint_requests_count",
            "cpu_endpoint_requests_count gets incremented on requests upon CPU endpoints.");
        counter.Inc();

        return Ok("CPU intensive process completed!");
    }

    private static void CalculatePrimes(int numberOfPrimes = 10)
    {
        for (int i = 2; i < numberOfPrimes; i++)
        {
            _ = IsPrime(i);
        }
    }

    private static bool IsPrime(int number)
    {
        if (number < 2) return false;

        for (int i = 2; i <= Math.Sqrt(number); i++)
        {
            if (number % i == 0) return false;
        }

        return true;
    }
}