﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Logging;
using Machine.Specifications;

namespace Dolittle.Specs.Logging.for_Logger
{
    public class when_logging_error : given.a_logger_and_reusable_details
    {
        Because of = () => logger.Error(message, file, line_number, member);

        It should_forward_to_appenders_with_level_of_error = () => appenders.Verify(a => a.Append(file, line_number, member, LogLevel.Error, message, null), Moq.Times.Once());
    }
}
