using ClinicSystem.DAL;
using ClinicSystem.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.BLL;

public class ReviewService : IReviewService
{
    private readonly IUnitOfWork _unitOfWork;
    public ReviewService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ReviewDto>> CreateReview(string patientUserId, CreateReviewDto dto)
    {
        var patient = await _unitOfWork.Patient.GetByUserId(patientUserId);
        if (patient == null) return Result<ReviewDto>.Failure("Patient profile not found.");

        var appointment = await _unitOfWork.Appointment.GetByIdWithDetails(dto.AppointmentId);
        if (appointment == null) return Result<ReviewDto>.Failure("Appointment not found.");

        if (appointment.PatientId != patient.Id)
            return Result<ReviewDto>.Failure("You can only review your own appointments.");

        if (appointment.Status != AppointmentStatus.Completed)
            return Result<ReviewDto>.Failure("You can only review completed appointments.");

        if (appointment.Review != null)
            return Result<ReviewDto>.Failure("A review has already been submitted for this appointment.");

        var newReview = new Review
        {
            AppointmentId = dto.AppointmentId,
            PatientId = patient.Id,
            DoctorId = appointment.DoctorId,
            Rating = dto.Rating,
            Comment = dto.Comment
        };

        await _unitOfWork.Review.Add(newReview); 
        await _unitOfWork.Complete();

        var reviewDto = new ReviewDto
        {
            Id = newReview.Id,
            Rating = newReview.Rating,
            Comment = newReview.Comment,
            CreatedAt = newReview.CreatedAt,
            PatientName = patient.FullName 
        };

        return Result<ReviewDto>.Success(reviewDto);
    }

    public async Task<Result<IEnumerable<ReviewDto>>> GetReviewsForDoctor(int doctorId)
    {
        var reviews = await _unitOfWork.Review.GetByDoctorId(doctorId);

        var reviewsDto = reviews.Select(r => new ReviewDto
        {
            Id = r.Id,
            Rating = r.Rating,
            Comment = r.Comment,
            CreatedAt = r.CreatedAt,
            PatientName = r.Patient!.FullName 
        });

        return Result<IEnumerable<ReviewDto>>.Success(reviewsDto);
    }
    public async Task<Result<ReviewDto>> UpdateMyReview(int reviewId, string patientUserId, UpdateReviewDto dto)
    {
        var patient = await _unitOfWork.Patient.GetByUserId(patientUserId);
        if (patient == null)
            return Result<ReviewDto>.Failure("Patient profile not found.");

        var review = await _unitOfWork.Review.GetByIdWithDetails(reviewId);
        if (review == null)
            return Result<ReviewDto>.Failure("Review not found.");

        if (review.PatientId != patient.Id)
            return Result<ReviewDto>.Failure("You are not authorized to edit this review.");

        review.Rating = dto.Rating;
        review.Comment = dto.Comment;
        review.UpdatedAt = DateTime.Now;

        await _unitOfWork.Complete();

        var reviewDto = new ReviewDto
        {
            Id = review.Id,
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt,
            PatientName = review.Patient!.FullName
        };

        return Result<ReviewDto>.Success(reviewDto);
    }
    public async Task<Result<bool>> DeleteMyReview(int reviewId, string patientUserId)
    {
        var patient = await _unitOfWork.Patient.GetByUserId(patientUserId);
        if (patient == null)
            return Result<bool>.Failure("Patient profile not found.");

        var review = await _unitOfWork.Review.GetById(reviewId);
        if (review == null)
            return Result<bool>.Failure("Review not found.");

        if (review.PatientId != patient.Id)
            return Result<bool>.Failure("You are not authorized to delete this review.");

        await _unitOfWork.Review.Delete(review);
        await _unitOfWork.Complete();

        return Result<bool>.Success(true);
    }
}
