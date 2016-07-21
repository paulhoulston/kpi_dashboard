INSERT INTO Deployments(ReleaseId, DeploymentDate, DeploymentStatus, Version, Comments)
VALUES (@releaseId, @deploymentDate, @deploymentStatus, @version, @comments);

SELECT SCOPE_IDENTITY();
