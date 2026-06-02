using System;
using System.Collections.Generic;
using TUBES_KPL.Core;
using TUBES_KPL.Models;

namespace TUBES_KPL.Core
{
    public class Result<T>
    {
        public bool Success { get; private set; }
        public T Data { get; private set; }
        public string Message { get; private set; }

        private Result(bool success, T data, string message)
        {
            Success = success;
            Data = data;
            Message = message;
        }

        public static Result<T> Ok(T data, string message = "")
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Data tidak boleh null");
            return new Result<T>(true, data, message);
        }

        public static Result<T> Fail(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Pesan error tidak boleh kosong");
            return new Result<T>(false, default, message);
        }
    }

    public class PagedList<T>
    {
        public int Page { get; private set; }
        public int Size { get; private set; }
        public int Total { get; private set; }
        public List<T> Items { get; private set; }

        public PagedList(List<T> items, int page, int size, int total)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            if (page < 1)
                throw new ArgumentOutOfRangeException(nameof(page), "Page minimal 1");
            if (size < 1)
                throw new ArgumentOutOfRangeException(nameof(size), "Size minimal 1");
            if (total < 0)
                throw new ArgumentOutOfRangeException(nameof(total), "Total tidak boleh negatif");

            Items = items;
            Page = page;
            Size = size;
            Total = total;
        }
    }

    public delegate void EventHandler<T>(T data, string eventName);

    public abstract class Repository<T>
    {
        protected List<T> storage = new List<T>();

        public virtual void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Item tidak boleh null");
            storage.Add(item);
        }

        public virtual List<T> GetAll()
        {
            return new List<T>(storage);
        }

        public abstract T GetById(int id);
    }

    public abstract class ValidationRule<T>
    {
        public abstract bool Validate(T input, out string errorMessage);

        public bool ValidateAll(T input, List<ValidationRule<T>> rules, out List<string> errors)
        {
            if (rules == null)
                throw new ArgumentNullException(nameof(rules));

            errors = new List<string>();
            foreach (var rule in rules)
            {
                if (!rule.Validate(input, out string msg))
                    errors.Add(msg);
            }
            return errors.Count == 0;
        }
    }

    public class RequiredStringRule : ValidationRule<string>
    {
        public override bool Validate(string input, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                errorMessage = "Field wajib diisi";
                return false;
            }
            errorMessage = "";
            return true;
        }
    }

    public class MaxLengthRule : ValidationRule<string>
    {
        private readonly int maxLength;

        public MaxLengthRule(int maxLength)
        {
            if (maxLength <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxLength), "MaxLength harus lebih dari 0");
            this.maxLength = maxLength;
        }

        public override bool Validate(string input, out string errorMessage)
        {
            if (input != null && input.Length > maxLength)
            {
                errorMessage = $"Panjang maksimal adalah {maxLength} karakter";
                return false;
            }
            errorMessage = "";
            return true;
        }
    }

    public class EnumValueRule<TEnum> : ValidationRule<string> where TEnum : struct, Enum
    {
        public override bool Validate(string input, out string errorMessage)
        {
            if (!Enum.TryParse<TEnum>(input, true, out _))
            {
                errorMessage = $"Nilai '{input}' tidak valid untuk tipe {typeof(TEnum).Name}";
                return false;
            }
            errorMessage = "";
            return true;
        }
    }

    public class ComplaintRepository : Repository<Complaint>
    {
        public override Complaint GetById(int id)
        {
            if (id < 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id tidak boleh negatif");
            if (id >= storage.Count)
                throw new ArgumentOutOfRangeException(nameof(id), "Id melebihi jumlah data");
            return storage[id];
        }
    }
}