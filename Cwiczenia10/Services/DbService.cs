using Cw10.DAL;
using Cw10.DTOs.Requests;
using Cw10.DTOs.Responses;
using Cwiczenia10.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cw10.DAL
{
    public class DbService : IDbService
    {
        private s14958Context _db = new s14958Context();


        public IEnumerable<Student> GetStudents()
        {
            return _db.Student.ToList();
        }

        public ICollection<string> GetStudentEnrollments(string IndexNumber)
        {
            return _db.Student
                .Where(student => student.IndexNumber == IndexNumber)
                .Join(_db.Enrollment, student => student.IdEnrollment, enrollment => enrollment.IdEnrollment, (student, enrollment) => enrollment)
                .Join(_db.Studies, enrollment => enrollment.IdStudy, study => study.IdStudy, (enrollment, study) => study.Name)
                .ToList();
        }

        public Enrollment EnrollStudent(EnrollStudentRequest request)
        {
            var studies = _db.Studies.Where(study => study.Name == request.Studies);
            if (studies.Count() != 1)
            {
                throw new Exception($"Nie ma studiów o podanej nazwie: {request.Studies}");
            }

            var studiesId = studies.First().IdStudy;

            var enrollment = _db.Enrollment
                .Where(enrollment =>
                    enrollment.IdStudy == studiesId && enrollment.Semester == 1
                )
                .FirstOrDefault();

            if (enrollment == null)
            {
                var newEnrollment = _db.Enrollment.Add(new Enrollment
                {
                    Semester = 1,
                    IdStudy = studiesId,
                    StartDate = DateTime.Now
                });

                _db.SaveChanges();

                enrollment = newEnrollment.Entity;
            }

            if (!(_db.Student.Find(request.IndexNumber) == null))
            {
                throw new Exception($"Student o indeksie {request.IndexNumber} istnieje już w bazie danych.");
            }

            var birthDate = DateTime.ParseExact(request.BirthDate, "dd.MM.yyyy", null);
            _db.Student.Add(new Student
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                IndexNumber = request.IndexNumber,
                BirthDate = birthDate,
                IdEnrollment = enrollment.IdEnrollment
            });

            _db.SaveChanges();

            return enrollment;
        }

        public Enrollment PromoteStudents(int semester, string studies)
        {
            var studyId = _db.Studies.Where(study => study.Name == studies).First().IdStudy;
            var enrollment = _db.Enrollment.Where(enrollment => enrollment.Semester == semester && enrollment.IdStudy == studyId).First();

            enrollment.Semester = enrollment.Semester + 1;
            _db.SaveChanges();

            return new Enrollment
            {
                IdEnrollment = enrollment.IdEnrollment,
                IdStudy = enrollment.IdStudy,
                Semester = enrollment.Semester,
                StartDate = enrollment.StartDate,
                Student = enrollment.Student
            };
        }
    }
}
