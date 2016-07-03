SELECT Id AS DeploymentId
     , ReleaseId
     , DeploymentDate
     , Version
     , DeploymentStatus
FROM Deployments
WHERE Id = @deploymentId