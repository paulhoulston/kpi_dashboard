using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using Optionis.KPIs.Dashboard.Application;

namespace Optionis.KPIs.DataAccess
{
    public class DeploymentCreator : DeploymentCreationService.ICreateDeployments
    {
        static readonly string _sql;
        static readonly string _connectionString = ConfigurationManager.ConnectionStrings["DeploymentsDb"].ConnectionString;

        static DeploymentCreator()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Optionis.KPIs.DataAccess.Database.CreateDeployment.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                _sql = reader.ReadToEnd();
            }
        }

        public void CreateDeployment(DeploymentCreationService.Deployment deployment, Action<int> onDeploymentCreated)
        {
            var deploymentId = new DbWrapper().ExecuteScalar(_sql, new
            {
                releaseId = deployment.ReleaseId,
                deploymentDate = deployment.DeploymentDate,
                deploymentStatus = deployment.Status,
                version = deployment.Version
            });
            onDeploymentCreated(deploymentId);
        }
    }
}
