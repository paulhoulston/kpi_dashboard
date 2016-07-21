SELECT Id AS DeploymentId
     , ReleaseId
     , DeploymentDate
     , Version
     , DeploymentStatus
     , Comments
FROM Deployments
WHERE Id = @deploymentId