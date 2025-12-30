using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valhaus.Data.DbInitializer;
public interface IDbInitializer
{
    void Initialize();
}

/*
    * pattern many developers use for initializing a database.
    
    * Why do we need IDbInitializer?

     * Ensure DB exists
            Automatically creates your database if it’s not there.

     * Seed initial data
            Roles (Admin/User)
            Default users

     * Consistency
            Every time the app starts in a new environment, the DB is ready.
            Avoids manual DB setup.
*/