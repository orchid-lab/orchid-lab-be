using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using orchid_backend_net.API.Controllers;
using orchid_backend_net.Application.MonitoringLog.Dto.AnalyticResult;
using orchid_backend_net.Application.MonitoringLog.Dto.Disease;
using orchid_backend_net.Application.MonitoringLog.UseCase.Analyze;
using Microsoft.VSDiagnostics;

namespace orchid_backend_net.API.Benchmarks;
[CPUUsageDiagnoser]
public class MonitoringLogUploadPathBenchmark
{
    private readonly MonitoringLogController _controller;
    private byte[] _inputImageBytes = Array.Empty<byte>();
    public MonitoringLogUploadPathBenchmark()
    {
        _controller = new MonitoringLogController(new FakeSender(), new NullLogger<MonitoringLogController>());
    }

    [GlobalSetup]
    public void Setup()
    {
        _inputImageBytes = Convert.FromBase64String("/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAkGBxAQEBAQEBAVFhUVFRUVFRUVFRUVFRUVFhUWFhUVFRUYHSggGBolGxUVITEhJSkrLi4uFx8zODMsNygtLisBCgoKDg0OGhAQGi0fHSUtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLf/AABEIAAEAAgMBIgACEQEDEQH/xAAXAAEBAQEAAAAAAAAAAAAAAAABAgAD/8QAFxEBAQEBAAAAAAAAAAAAAAAAAQARIf/aAAwDAQACEAMQAAAB9SEAAAAAAAAAAP/EABQQAQAAAAAAAAAAAAAAAAAAACD/2gAIAQEAAQUCcf/EABQRAQAAAAAAAAAAAAAAAAAAACD/2gAIAQMBAT8BJ//EABQRAQAAAAAAAAAAAAAAAAAAACD/2gAIAQIBAT8BJ//EABQQAQAAAAAAAAAAAAAAAAAAACD/2gAIAQEABj8Cf//Z");
    }

    [Benchmark]
    public async Task<IActionResult> Analytic_UploadPath_Current()
    {
        var file = new FormFile(new MemoryStream(_inputImageBytes), 0, _inputImageBytes.Length, "image", "sample.jpg")
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/jpeg"
        };
        return await _controller.Analytic(file, CancellationToken.None);
    }

    private sealed class FakeSender : ISender
    {
        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            if (request is AnalyzeOrchidImageCommand)
            {
                var response = new AnalyticResultAfterAnalysisDto
                {
                    StageName = "Coppice",
                    Disease = new DiseaseDto
                    {
                        Id = 1,
                        Name = "Healthy",
                        Code = "disease_healthy",
                        Description = "Benchmark"
                    },
                    AnalyticResult = new AnalyticResultDto
                    {
                        Id = "1",
                        Anthracnose = 0,
                        BacterialWilt = 0,
                        Blackrot = 0,
                        Brownspots = 0,
                        MoldBacterial = 0,
                        MoldFungus = 0,
                        SoftRot = 0,
                        StemRot = 0,
                        WitheredYellowRoot = 0,
                        Healthy = 1,
                        Oxidation = 0,
                        Virus = 0
                    }
                };
                return Task.FromResult((TResponse)(object)response);
            }

            throw new InvalidOperationException($"Unexpected request type: {request.GetType().Name}");
        }

        public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : IRequest
        {
            return Task.CompletedTask;
        }

        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            return EmptyStream<TResponse>();
        }

        public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default)
        {
            return EmptyStream<object?>();
        }

        private static async IAsyncEnumerable<T> EmptyStream<T>()
        {
            await Task.CompletedTask;
            yield break;
        }

        public Task<object?> Send(object request, CancellationToken cancellationToken = default)
        {
            if (request is AnalyzeOrchidImageCommand command)
            {
                return Task.FromResult<object?>(new AnalyticResultAfterAnalysisDto { StageName = "Coppice", Disease = new DiseaseDto { Id = 1, Name = "Healthy", Code = "disease_healthy", Description = "Benchmark" }, AnalyticResult = new AnalyticResultDto { Id = "1", Anthracnose = 0, BacterialWilt = 0, Blackrot = 0, Brownspots = 0, MoldBacterial = 0, MoldFungus = 0, SoftRot = 0, StemRot = 0, WitheredYellowRoot = 0, Healthy = 1, Oxidation = 0, Virus = 0 } });
            }

            throw new InvalidOperationException($"Unexpected request type: {request.GetType().Name}");
        }
    }
}