using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TUBES_KPL.Core;
using TUBES_KPL.Models;

namespace TUBES_KPL.Testing
{
    [TestClass]
    public class GenericsTest
    {
        [TestMethod]
        public void Result_Ok_ReturnsSuccess()
        {
            var complaint = new Complaint("Jalan rusak", "Infrastruktur", "Berat", "Berlubang", "Bandung", "Budi");
            var result = Result<Complaint>.Ok(complaint, "Berhasil");
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void Result_Fail_ReturnsFailure()
        {
            var result = Result<Complaint>.Fail("Tidak ditemukan");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Tidak ditemukan", result.Message);
        }

        [TestMethod]
        public void Result_Fail_EmptyMessage_Throws()
        {
            bool threw = false;
            try
            {
                Result<Complaint>.Fail("");
            }
            catch (ArgumentException)
            {
                threw = true;
            }
            Assert.IsTrue(threw);
        }

        [TestMethod]
        public void PagedList_ValidInput_Works()
        {
            var items = new List<Complaint>
            {
                new Complaint("Sampah", "Kebersihan", "Ringan", "Menumpuk", "Surabaya", "Siti")
            };
            var paged = new PagedList<Complaint>(items, 1, 10, 1);
            Assert.AreEqual(1, paged.Page);
            Assert.AreEqual(1, paged.Items.Count);
        }

        [TestMethod]
        public void PagedList_InvalidPage_Throws()
        {
            bool threw = false;
            try
            {
                new PagedList<Complaint>(new List<Complaint>(), 0, 10, 0);
            }
            catch (ArgumentOutOfRangeException)
            {
                threw = true;
            }
            Assert.IsTrue(threw);
        }

        [TestMethod]
        public void RequiredStringRule_Empty_Fails()
        {
            var rule = new RequiredStringRule();
            bool valid = rule.Validate("", out string msg);
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void RequiredStringRule_Valid_Passes()
        {
            var rule = new RequiredStringRule();
            bool valid = rule.Validate("Infrastruktur", out string msg);
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void MaxLengthRule_TooLong_Fails()
        {
            var rule = new MaxLengthRule(5);
            bool valid = rule.Validate("Terlalu panjang banget ini", out string msg);
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void MaxLengthRule_Valid_Passes()
        {
            var rule = new MaxLengthRule(50);
            bool valid = rule.Validate("Oke", out string msg);
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void EnumValueRule_InvalidValue_Fails()
        {
            var rule = new EnumValueRule<ComplaintStatus>();
            bool valid = rule.Validate("StatusTidakAda", out string msg);
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void EnumValueRule_ValidValue_Passes()
        {
            var rule = new EnumValueRule<ComplaintStatus>();
            bool valid = rule.Validate("Diajukan", out string msg);
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void ComplaintRepository_AddAndGet_Works()
        {
            var repo = new ComplaintRepository();
            var c = new Complaint("Banjir", "Infrastruktur", "Berat", "Meluap", "Jakarta", "Andi");
            repo.Add(c);
            var result = repo.GetById(0);
            Assert.AreEqual("Banjir", result.Title);
        }

        [TestMethod]
        public void Repository_AddNull_Throws()
        {
            bool threw = false;
            try
            {
                var repo = new ComplaintRepository();
                repo.Add(null);
            }
            catch (ArgumentNullException)
            {
                threw = true;
            }
            Assert.IsTrue(threw);
        }

        [TestMethod]
        public void ComplaintRepository_InvalidId_Throws()
        {
            bool threw = false;
            try
            {
                var repo = new ComplaintRepository();
                repo.GetById(99);
            }
            catch (ArgumentOutOfRangeException)
            {
                threw = true;
            }
            Assert.IsTrue(threw);
        }
    }
}