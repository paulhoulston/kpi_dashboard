SELECT TOP {=top} ReleaseId
FROM
(
    SELECT ReleaseId, MAX(DeploymentDate)
    FROM Deployments
    GROUP BY DeploymentDate
)