﻿namespace LibraryApp
{
    public class Book
    {
        //public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public Book() { }
        public Book(string title, string author)
        {
            Title = title;
            Author = author;
        }
    }
}
