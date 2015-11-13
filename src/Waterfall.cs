using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;

	public class Waterfall
    {
        public class Context
        {
            private AutoResetEvent ev { get; set; }
            private bool requestAborted { get; set; }

            public Context()
            {
                this.ev = new AutoResetEvent(false);
                this.requestAborted = false;
            }

            public void Next()
            {
                ev.Set();
            }
            public void Abort(String msg)
            {
                Debug.LogError(msg);

                requestAborted = true;

                ev.Set();
            }
            public void Abort(Exception e)
            {
                Debug.LogException(e);

                requestAborted = true;

                ev.Set();
            }

            public bool Wait()
            {
                ev.WaitOne();

                return requestAborted;
            }
        }

        private List<Action<Context>> chain { get; set; }
        private Context context { get; set; }

        private Waterfall(Action<Context> action)
        {
            chain = new List<Action<Context>>();
            context = new Context();
            chain.Add(action);
        }

        public static Waterfall Begin(Action<Context> action)
        {
            Waterfall w = new Waterfall(action);
            return w;
        }
        public Waterfall Then(Action<Context> action)
        {
            chain.Add(action);
            return this;
        }

        public void Run()
        {
            foreach(var action in chain)
            {
                action(context);

                if (context.Wait())
                    break;
            }
        }
    }