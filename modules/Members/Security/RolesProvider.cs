namespace lw.Members.Security
{
 
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Utils;
    using lw.CTE.Enum;
 
    /// <summary>
    /// Represents the Role provider for the ASP.Net web application
    /// </summary>
    public sealed class CustomRolesProvider : System.Web.Security.RoleProvider
    {
        MembersManager manager = new MembersManager();

        /// <summary>
        /// Represents the Roles Provider application Name
        /// </summary>
        internal const string CustomApplicationName = "Custom";

        /// <summary>
        /// Represents the application roles
        /// </summary>
        private static List<string> applicationRoles;

        /// <summary>
        /// Initializes static members of the <see cref="CustomRolesProvider"/> class.
        /// </summary>
        static CustomRolesProvider()
        {
            // Retrieving the application roles
            applicationRoles = new List<string>();
            Type type = typeof(Roles);
            foreach (System.Reflection.FieldInfo info in type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                applicationRoles.Add(info.Name);
            }
        }

        /// <summary>
        /// Gets or sets the name of the application to store and retrieve role information for.
        /// </summary>
        /// <value>The application name</value>
        /// <returns>The name of the application to store and retrieve role information for.</returns>
        public override string ApplicationName
        {
            get
            {
                return CustomApplicationName;
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets a list of all the roles for the configured applicationName.
        /// </summary>
        /// <returns>
        /// A string array containing the names of all the roles stored in the data source for the configured applicationName.
        /// </returns>
        public override string[] GetAllRoles()
        {
            return applicationRoles.ToArray();
        }

        /// <summary>
        /// Gets a value indicating whether the specified role name already exists in the role data source for the configured applicationName.
        /// </summary>
        /// <param name="roleName">The name of the role to search for in the data source.</param>
        /// <returns>
        /// true if the role name already exists in the data source for the configured applicationName; otherwise, false.
        /// </returns>
        public override bool RoleExists(string roleName)
        {
            string[] allRoles = this.GetAllRoles();
            bool retVal = allRoles.Contains<string>(roleName);
            return retVal;
        }

        /// <summary>
        /// Gets a list of the roles that a specified user is in for the configured applicationName.
        /// </summary>
        /// <param name="username">The user to return a list of roles for.</param>
        /// <returns>
        /// A string array containing the names of all the roles that the specified user is in for the configured applicationName.
        /// </returns>
        public override string[] GetRolesForUser(string username)
        {
            MembersDs.MembersRow member = null;
            try
            {
                member = manager.GetMember(username);
                string memberRole = member.Roles.ToString().Replace(" ", string.Empty);
                string[] allRoles = memberRole.Split(",".ToCharArray());
                return allRoles;
            }
            catch (Exception)
            {
                return new string[0];
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified user is in the specified role for the configured applicationName.
        /// </summary>
        /// <param name="username">The user name to search for.</param>
        /// <param name="roleName">The role to search in.</param>
        /// <returns>
        /// true if the specified user is in the specified role for the configured applicationName; otherwise, false.
        /// </returns>
        public override bool IsUserInRole(string username, string roleName)
        {
            try
            {
                MembersDs.MembersRow member = manager.GetMember(username);
                Roles role;
                bool isValidRole = Enum.TryParse<Roles>(roleName, out role);
                if (!isValidRole)
                {
                    return false;
                }

                bool isMember = (member.Roles.EnsureEnum<Roles>() & role) == role;
                return isMember;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Adds the specified user names to the specified roles for the configured applicationName.
        /// </summary>
        /// <param name="usernames">A string array of user names to be added to the specified roles.</param>
        /// <param name="roleNames">A string array of the role names to add the specified user names to.</param>
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            Roles allRoles = Roles.Visitor;
            Roles role;
            foreach (string roleName in roleNames)
            {
                role = (Roles)Enum.Parse(typeof(Roles), roleName);
                allRoles = allRoles | role;
            }

            MembersDs.MembersRow member;

            foreach (string username in usernames)
            {
                member = manager.GetMember(username);
                Roles roles = allRoles;
                MembersManager.UpdateRoles(member.MemberId, roles);
            }
        }

        /// <summary>
        /// Removes the specified user names from the specified roles for the configured applicationName.
        /// </summary>
        /// <param name="usernames">A string array of user names to be removed from the specified roles.</param>
        /// <param name="roleNames">A string array of role names to remove the specified user names from.</param>
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            Roles allRoles = Roles.Visitor;
            Roles role;
            foreach (string roleName in roleNames)
            {
                role = (Roles)Enum.Parse(typeof(Roles), roleName);
                allRoles = allRoles | role;
            }

            MembersDs.MembersRow member;

            foreach (string username in usernames)
            {
                member = manager.GetMember(username);
                Roles roles = member.Roles.EnsureEnum<Roles>() ^ allRoles;
                MembersManager.UpdateRoles(member.MemberId, roles);
            }
        }

        #region Not Supported
        /// <summary>
        /// Adds a new role to the data source for the configured applicationName.
        /// </summary>
        /// <param name="roleName">The name of the role to create.</param>
        public override void CreateRole(string roleName)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Removes a role from the data source for the configured applicationName.
        /// </summary>
        /// <param name="roleName">The name of the role to delete.</param>
        /// <param name="throwOnPopulatedRole">If true, throw an exception if <paramref name="roleName"/> has one or more members and do not delete <paramref name="roleName"/>.</param>
        /// <returns>
        /// true if the role was successfully deleted; otherwise, false.
        /// </returns>
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets a list of users in the specified role for the configured applicationName.
        /// </summary>
        /// <param name="roleName">The name of the role to get the list of users for.</param>
        /// <returns>
        /// A string array containing the names of all the users who are members of the specified role for the configured applicationName.
        /// </returns>
        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets an array of user names in a role where the user name contains the specified user name to match.
        /// </summary>
        /// <param name="roleName">The role to search in.</param>
        /// <param name="usernameToMatch">The user name to search for.</param>
        /// <returns>
        /// A string array containing the names of all the users where the user name matches <paramref name="usernameToMatch"/> and the user is a member of the specified role.
        /// </returns>
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotSupportedException();
        }
        #endregion

       
    }
}
