SELECT TOP {=top} ReleaseId
FROM
(
    SELECT ReleaseId, MAX(DeploymentDate) AS DeploymentDate
    FROM Deployments
    GROUP BY ReleaseId
) t
ORDER BY t.DeploymentDate DESC