﻿using Cw10.DTOs.Requests;
using Cwiczenia10.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw10.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents();
        public ICollection<string> GetStudentEnrollments(string IndexNumber);

        Enrollment EnrollStudent(EnrollStudentRequest request);
        Enrollment PromoteStudents(int semester, string studies);
    }
}
