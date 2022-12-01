using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models.DTO;

namespace WorkForceManagementV0.Repositories.ActiveDirectory
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
    public class ActiveDirectory:IActiveDirectory
    {
        
        public List<string> GetGroups()
        {
            SearchResultCollection results;
            DirectorySearcher ds = null;
            DirectoryEntry de = new DirectoryEntry(GetCurrentDomainPath());
            ds = new DirectorySearcher(de);
            // Sort by name
            ds.Sort = new SortOption("name", SortDirection.Ascending);
            ds.PropertiesToLoad.Add("name");
            ds.PropertiesToLoad.Add("memberof");
            ds.PropertiesToLoad.Add("member");

            ds.Filter = "(&(objectCategory=Group))";
            results = ds.FindAll();
            var final = new List<string>();
            foreach(SearchResult sr in results)
            {
                if (sr.Properties["name"].Count > 0)
                {
                    final.Add(sr.Properties["name"][0].ToString());
                }
            }
            return final;
        }
        public List<string> GetGroupsBySearch(string searchQuery)
        {
            SearchResultCollection results;
            DirectorySearcher ds = null;
            DirectoryEntry de = new DirectoryEntry(GetCurrentDomainPath());
            ds = new DirectorySearcher(de);
            // Sort by name
            ds.Sort = new SortOption("name", SortDirection.Ascending);
            ds.PropertiesToLoad.Add("name");
            ds.PropertiesToLoad.Add("memberof");
            ds.PropertiesToLoad.Add("member");

            ds.Filter = "(&(objectCategory=Group)(name=*"+searchQuery+"*))";
            results = ds.FindAll();
            var final = new List<string>();
            foreach (SearchResult sr in results)
            {
                if (sr.Properties["name"].Count > 0)
                {
                    final.Add(de.Name.Replace("DC=","").Trim() + "\\" + sr.Properties["name"][0].ToString());
                }
            }
            return final;
        }

        public List<ADUser> GetUsers()
        {
            SearchResultCollection results;
            DirectorySearcher ds = null;
            DirectoryEntry de = new DirectoryEntry(GetCurrentDomainPath());
            ds = new DirectorySearcher(de);
            // Sort by name
            ds.Sort = new SortOption("name", SortDirection.Ascending);
            ds.PropertiesToLoad.Add("name");
            ds.PropertiesToLoad.Add("sAMAccountName");

            ds.Filter = "(&(objectCategory=User)(objectClass=person))";
            results = ds.FindAll();
            var final = new List<ADUser>();
            foreach (SearchResult sr in results)
            {
                if (sr.Properties["sAMAccountName"].Count > 0)
                {
                    final.Add(new ADUser { Name = sr.Properties["sAMAccountName"][0].ToString(), Alias = sr.Properties["name"][0].ToString() });
                }
            }
            return final;
        }

        public List<ADUser> GetUsersBySearch(string searchQuery)
        {
            SearchResultCollection results;
            DirectorySearcher ds = null;
            DirectoryEntry de = new DirectoryEntry(GetCurrentDomainPath());
            ds = new DirectorySearcher(de);
            // Sort by name
            ds.Sort = new SortOption("name", SortDirection.Ascending);
            ds.PropertiesToLoad.Add("name");
            ds.PropertiesToLoad.Add("sAMAccountName");

            ds.Filter = "(&(objectCategory=User)(objectClass=person)(name=*" + searchQuery + "*))";
            results = ds.FindAll();
            var final = new List<ADUser>();
            foreach (SearchResult sr in results)
            {
                if (sr.Properties["name"].Count > 0)
                {
                    final.Add(new ADUser { Alias = sr.Properties["sAMAccountName"][0].ToString(), Name = sr.Properties["name"][0].ToString() });
                }
            }
            return final;
        }

        private string GetCurrentDomainPath()
        {
            DirectoryEntry de = new DirectoryEntry("LDAP://RootDSE");

            return "LDAP://" + de.Properties["defaultNamingContext"][0].ToString();
        }
    }
}
