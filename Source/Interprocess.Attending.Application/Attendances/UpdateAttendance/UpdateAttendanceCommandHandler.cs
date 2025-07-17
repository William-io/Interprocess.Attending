using Interprocess.Attending.Application.Abstractions.MessageCommunication;
using Interprocess.Attending.Domain.Abstractions;
using Interprocess.Attending.Domain.Attendances;

namespace Interprocess.Attending.Application.Attendances.UpdateAttendance;

internal sealed class UpdateAttendanceCommandHandler : ICommandHandler<UpdateAttendanceCommand>
{
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAttendanceCommandHandler(
        IAttendanceRepository attendanceRepository,
        IUnitOfWork unitOfWork)
    {
        _attendanceRepository = attendanceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateAttendanceCommand request, CancellationToken cancellationToken)
    {
        var attendance = await _attendanceRepository.GetByIdAsync(request.AttendanceId, cancellationToken);

        if (attendance is null)
        {
            return Result.Failure(AttendanceErrors.NotFound);
        }

        var updateResult = attendance.Update(request.Description, request.CreatedOnUtc);

        if (updateResult.IsFailure)
        {
            return updateResult;
        }

        _attendanceRepository.Update(attendance);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
