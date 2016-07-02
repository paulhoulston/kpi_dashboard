INSERT INTO Deployments(ReleaseId, DeploymentDate, DeploymentStatus, Version)
VALUES (@releaseId, @deploymentDate, @deploymentStatus, @version);

SELECT SCOPE_IDENTITY();
