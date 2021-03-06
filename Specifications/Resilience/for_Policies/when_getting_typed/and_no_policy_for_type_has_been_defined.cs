// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Machine.Specifications;

namespace Dolittle.Resilience.Specs.for_Policies.when_getting_typed
{
    public class and_no_policy_for_type_has_been_defined : given.defined_default_policy
    {
        static PolicyFor<string> policy;

        Because of = () => policy = policies.GetFor<string>() as PolicyFor<string>;

        It should_return_policy_that_delegates_to_default_policy = () => policy.DelegatedPolicy.ShouldEqual(policies.Default);
    }
}