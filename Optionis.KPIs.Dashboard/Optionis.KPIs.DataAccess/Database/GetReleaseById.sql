SELECT 
      r.Id
    , r.Application
    , r.Comments
    , r.Created
    , r.CreatedBy
    , r.Title
FROM Releases r 
WHERE r.Id = @releaseId

SELECT Id AS DeploymentId
FROM Deployments
WHERE ReleaseId = @releaseId

SELECT Id AS IssueId
FROM Issues
WHERE ReleaseId = @releaseId