﻿using Interprocess.Attending.Application.Abstractions.MessageCommunication;

namespace Interprocess.Attending.Application.Attendances.GetAttendance;

public sealed record GetAttendanceQuery() : IQuery<IEnumerable<AttendanceResponse>>;