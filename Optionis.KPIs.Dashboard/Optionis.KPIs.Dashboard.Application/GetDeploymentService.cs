﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Optionis.KPIs.Dashboard.Application
{
    public class GetDeploymentService
    {
        readonly Action _onDeploymentNotFound;
        readonly Action<Deployment> _onDeploymentFound;
        readonly IGetDeployments _repository;

        public interface IGetDeployments
        {
            void Get(int deploymentId, Action onDeploymentNotFound, Action<Deployment> onDeploymentFound);
        }

        [DefaultValue(Unknown)]
        public enum DeploymentStatus
        {
            Unknown = 0,
            Pending = 1,
            Success = 2,
            Failed = 3,
            Aborted = 4
        }

        public class Deployment
        {
            public int Id { get; set; }
            public DateTime DeploymentDate { get; set; }
            public string Version { get; set; }
            public DeploymentStatus Status { get; set; }
        }

        public GetDeploymentService (IGetDeployments repository, Action onDeploymentNotFound, Action<Deployment> onDeploymentFound)
        {
            _repository = repository;
            _onDeploymentNotFound = onDeploymentNotFound;
            _onDeploymentFound = onDeploymentFound;
        }

        public void Get (int deploymentId)
        {
            _repository.Get (deploymentId, _onDeploymentNotFound, _onDeploymentFound);
        }
    }
}
