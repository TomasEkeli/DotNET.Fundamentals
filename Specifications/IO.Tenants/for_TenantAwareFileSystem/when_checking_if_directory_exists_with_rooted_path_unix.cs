using System;
using Dolittle.Execution;
using Machine.Specifications;
using Moq;
using It=Machine.Specifications.It;

namespace Dolittle.IO.Tenants.for_TenantAwareFileSystem
{
    public class when_checking_if_directory_exists_with_rooted_path_unix : given.a_tenant_aware_file_system
    {
        static Exception result;

        Because of = () => result = Catch.Exception(() => tenant_aware_file_system.DirectoryExists("/someplace"));

        It should_throw_access_outside_sandbox_denied = () => result.ShouldBeOfExactType<AccessOutsideSandboxDenied>();
    }
}