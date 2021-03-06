using System;

namespace conole001
{
    interface IO<A>
    {
        IO<B> Bind<B>(Func<A, IO<B>> f);
    }
    sealed class Return<A> : IO<A>
    {
        public A Value { get; }
        public Return(A value) => Value = value;

        public IO<B> Bind<B>(Func<A, IO<B>> f) => f(Value);
    }
    class IO<I, O, A> : IO<A>
    {
        public I Input { get; }
        public Func<O, IO<A>> Next { get; }
        public IO(I input, Func<O, IO<A>> next) => (Input, Next) = (input, next);
        public IO<B> Bind<B>(Func<A, IO<B>> f) => new IO<I, O, B>(Input, a => Next(a).Bind(f));
    }
    static class IOMonad
    {
        public static IO<A> Lift<A>(this A a) => new Return<A>(a);
        public static IO<B> Select<A, B>(this IO<A> m, Func<A, B> f) => m.Bind(a => f(a).Lift());
        public static IO<C> SelectMany<A, B, C>(this IO<A> m, Func<A, IO<B>> f, Func<A, B, C> project)
        => m.Bind(a => f(a).Bind(b => project(a, b).Lift()));
    }
    static class IOMonadSugar
    {
        public static IO<O> ToIO<I, O>(this I input) => new IO<I, O, O>(input, IOMonad.Lift);
    }
}