﻿using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphView.TSQL_Syntax_Tree;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace GraphView
{
    class GraphTraversal : IEnumerable<Record>
    {
        internal enum direction
        {
            In,
            Out,
            Undefine
        }

        public class GraphTraversalIterator :IEnumerator<Record>
        {
            private GraphViewOperator CurrentOperator;
            internal GraphTraversalIterator(GraphViewOperator pCurrentOperator)
            {
                CurrentOperator = pCurrentOperator;
                elements = new List<int>();
            }
            private Func<GraphViewGremlinSematicAnalyser.Context> Modifier;
            internal Record CurrentRecord;
            internal List<int> elements;
            public bool MoveNext()
            {
                if (CurrentOperator == null) Reset();

                if (CurrentOperator.Status())
                {
                    CurrentRecord = CurrentOperator.Next();
                    return true;
                }
                else return false;
            }

            public void Reset()
            {
            }

            object IEnumerator.Current
            {
                get
                {
                    return CurrentRecord;
                }
            }

            public Record Current
            {
                get
                {
                    return CurrentRecord;
                }
            }

            public void Dispose()
            {
                
            }
        }

        internal GraphViewOperator CurrentOperator;
        internal GraphTraversalIterator it;
        internal GraphViewConnection connection;
        internal List<int> TokenIndex;
        internal string AppendExecutableString;
        internal GraphTraversal AddEdgeOtherSource;
        internal bool HoldMark;
        internal List<string> elements;
        internal direction dir;

        internal static GraphTraversal held;

        public List<Record> ToList()
        {
            List<Record> RecordList = new List<Record>(); 
            foreach (var x in this)
                RecordList.Add(x);
            return RecordList;
        }

        public void Invoke()
        {
            if (it == null)
            {
                if (CurrentOperator == null)
                {
                    if (AddEdgeOtherSource != null)
                    {

                    }
                    GraphViewGremlinParser parser = new GraphViewGremlinParser();
                    CurrentOperator = parser.Parse(CutTail(AppendExecutableString)).Generate(connection);
                    it = new GraphTraversalIterator(CurrentOperator);
                    foreach (var x in parser.elements) it.elements.Add(CurrentOperator.header.IndexOf(x));
                }
            }
            while (CurrentOperator.Status()) CurrentOperator.Next();
        }

        public IEnumerator<Record> GetEnumerator()
        {
            if (it == null)
            {
                if (CurrentOperator == null)
                {
                    if (AddEdgeOtherSource != null)
                    {
                        
                    }
                    GraphViewGremlinParser parser = new GraphViewGremlinParser();
                    CurrentOperator = parser.Parse(CutTail(AppendExecutableString)).Generate(connection);
                    it = new GraphTraversalIterator(CurrentOperator);
                    foreach (var x in parser.elements) it.elements.Add(CurrentOperator.header.IndexOf(x));
                }
            }
            return it;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public GraphTraversal(GraphTraversal rhs)
        {
            CurrentOperator = rhs.CurrentOperator;
            AppendExecutableString = rhs.AppendExecutableString;
            HoldMark = rhs.HoldMark;
            TokenIndex = rhs.TokenIndex;
            connection = rhs.connection;
            AddEdgeOtherSource = rhs.AddEdgeOtherSource;
            dir = rhs.dir;
        }

        public GraphTraversal(ref GraphViewConnection pConnection)
        {
            CurrentOperator = null;
            AppendExecutableString = "";
            HoldMark = true;
            TokenIndex = new List<int>();
            connection = pConnection;
            dir = direction.Undefine;
        }

        public GraphTraversal(GraphTraversal rhs, string NewAES)
        {
            CurrentOperator = rhs.CurrentOperator;
            AppendExecutableString = NewAES;
            HoldMark = rhs.HoldMark;
            TokenIndex = rhs.TokenIndex;
            connection = rhs.connection;
            AddEdgeOtherSource = rhs.AddEdgeOtherSource;
            dir = rhs.dir;
        }
        private int index;
        private string SrcNode;
        private string DestNode;
        private string Edge;
        private string Parameter;
        private List<string> StatueKeeper = new List<string>();
        private List<string> NewPrimaryInternalAlias = new List<string>();

        public static Tuple<int, GraphViewGremlinParser.Keywords> lt(int i)
        {
            return new Tuple<int, GraphViewGremlinParser.Keywords>(i, GraphViewGremlinParser.Keywords.lt);
        }
        public static Tuple<int, GraphViewGremlinParser.Keywords> gt(int i)
        {
            return new Tuple<int, GraphViewGremlinParser.Keywords>(i, GraphViewGremlinParser.Keywords.gt);

        }
        public static Tuple<int, GraphViewGremlinParser.Keywords> eq(int i)
        {
            return new Tuple<int, GraphViewGremlinParser.Keywords>(i, GraphViewGremlinParser.Keywords.eq);

        }
        public static Tuple<int, GraphViewGremlinParser.Keywords> lte(int i)
        {
            return new Tuple<int, GraphViewGremlinParser.Keywords>(i, GraphViewGremlinParser.Keywords.lte);

        }

        public static Tuple<int, GraphViewGremlinParser.Keywords> gte(int i)
        {
            return new Tuple<int, GraphViewGremlinParser.Keywords>(i, GraphViewGremlinParser.Keywords.gte);

        }

        public static Tuple<int, GraphViewGremlinParser.Keywords> neq(int i)
        {
            return new Tuple<int, GraphViewGremlinParser.Keywords>(i, GraphViewGremlinParser.Keywords.neq);

        }
        public static Tuple<string, GraphViewGremlinParser.Keywords> lt(string i)
        {
            return new Tuple<string, GraphViewGremlinParser.Keywords>(i, GraphViewGremlinParser.Keywords.lt);
        }
        public static Tuple<string, GraphViewGremlinParser.Keywords> gt(string i)
        {
            return new Tuple<string, GraphViewGremlinParser.Keywords>(i, GraphViewGremlinParser.Keywords.gt);

        }
        public static Tuple<string, GraphViewGremlinParser.Keywords> eq(string i)
        {
            return new Tuple<string, GraphViewGremlinParser.Keywords>(i, GraphViewGremlinParser.Keywords.eq);

        }
        public static Tuple<string, GraphViewGremlinParser.Keywords> lte(string i)
        {
            return new Tuple<string, GraphViewGremlinParser.Keywords>(i, GraphViewGremlinParser.Keywords.lte);

        }

        public static Tuple<string, GraphViewGremlinParser.Keywords> gte(string i)
        {
            return new Tuple<string, GraphViewGremlinParser.Keywords>(i, GraphViewGremlinParser.Keywords.gte);

        }

        public static Tuple<string, GraphViewGremlinParser.Keywords> neq(string i)
        {
            return new Tuple<string, GraphViewGremlinParser.Keywords>(i, GraphViewGremlinParser.Keywords.neq);
        }

        public static Tuple<string[], GraphViewGremlinParser.Keywords> within(params string[] i)
        {
            return new Tuple<string[], GraphViewGremlinParser.Keywords>(i, GraphViewGremlinParser.Keywords.within);
        }

        public static Tuple<string[], GraphViewGremlinParser.Keywords> without(params string[] i)
        {
            return new Tuple<string[], GraphViewGremlinParser.Keywords>(i, GraphViewGremlinParser.Keywords.without);
        }

        public static string incr()
        {
            return "incr";
        }

        public static string decr()
        {
            return "decr";
        }

        public static GraphTraversal _underscore()
        {
            GraphViewConnection NullConnection = new GraphViewConnection();
            GraphTraversal HeldPipe = new GraphTraversal(ref NullConnection);
            HeldPipe.HoldMark = false;
            HeldPipe.AppendExecutableString += "__.";
            return HeldPipe;
        }
        public GraphTraversal V()
        {
            if (HoldMark == true) held = this;

            return new GraphTraversal(this, AppendExecutableString + "V().");
        }

        public GraphTraversal E()
        {
            if (HoldMark == true) held = this;

            return new GraphTraversal(this, AppendExecutableString + "E().");

        }

        public GraphTraversal next()
        {
            if (HoldMark == true) held = this;

            return new GraphTraversal(this, AppendExecutableString + "next().");

        }

        public GraphTraversal has(string name, string value)
        {
            if (HoldMark == true) held = this;

            return new GraphTraversal(this, AppendExecutableString + "has(\'" + name + "\', " + "\'" + value + "\').");

        }

        public GraphTraversal has(string name, Tuple<int, GraphViewGremlinParser.Keywords> ComparisonFunc)
        {
            Tuple<int, GraphViewGremlinParser.Keywords> des = ComparisonFunc;
            string AES = AppendExecutableString;
            AES += "has(\'" + name + "\', ";
                switch (des.Item2)
                {
                    case GraphViewGremlinParser.Keywords.lt:
                        AES += "lt("+des.Item1+ ")";
                        break;
                    case GraphViewGremlinParser.Keywords.gt:
                        AES += "gt(" + des.Item1 + ")";
                        break;
                    case GraphViewGremlinParser.Keywords.eq:
                        AES += "eq(" + des.Item1 + ")";
                        break;
                    case GraphViewGremlinParser.Keywords.lte:
                        AES += "lte(" + des.Item1 + ")";
                        break;
                    case GraphViewGremlinParser.Keywords.gte:
                        AES += "gte(" + des.Item1 + ")";
                        break;
                    case GraphViewGremlinParser.Keywords.neq:
                        AES += "neq(" + des.Item1 + ")";
                        break;
                  }
            AES += ").";
            if (HoldMark == true) held = this;

            return new GraphTraversal(this, AES);
        }
        public GraphTraversal has(string name, Tuple<string[], GraphViewGremlinParser.Keywords> ComparisonFunc)
        {
            Tuple<string[], GraphViewGremlinParser.Keywords> des = ComparisonFunc;
            string AES = AppendExecutableString;
            AES += "has(\'" + name + "\', ";
            switch (des.Item2)
            {
                case GraphViewGremlinParser.Keywords.within:
                    AES += "within(\'" + String.Join("\',\'", des.Item1) + "\')";
                    break;
                case GraphViewGremlinParser.Keywords.without:
                    AES += "without(\'" + String.Join("\',\'", des.Item1) + "\')";
                    break;
            }
            AES += ").";
            if (HoldMark == true) held = this;

            return new GraphTraversal(this, AES);
        }

        public GraphTraversal Out(params string[] Parameters)
        {
            string AES = AppendExecutableString;
            if (Parameters == null)
                AES += "out().";
            else
                AES += "out(\'"+string.Join("\', \'",Parameters)+"\').";
            if (HoldMark == true) held = this;

            return new GraphTraversal(this, AES);

        }

        public GraphTraversal In(params string[] Parameters)
        {
            string AES = AppendExecutableString;
            if (Parameters == null)
                AES += "in().";
            else
                AES += "in(\'" + string.Join("\', \'", Parameters) + "\').";
            if (HoldMark == true) held = this;

            return new GraphTraversal(this, AES);

        }

        public GraphTraversal outE(params string[] Parameters)
        {
            string AES = AppendExecutableString;
            if (Parameters != null)
            {
                List<string> StringList = new List<string>();
                AES += "outE(";
                foreach (var x in Parameters)
AES += "\'" + x + "\'";
                AES += ").";
            }
            else
                AES += "outE().";
            if (HoldMark == true) held = this;

            return new GraphTraversal(this);

        }

        public GraphTraversal inE(params string[] Parameters)
        {
            string AES = AppendExecutableString;
            if (Parameters != null)
            {
                List<string> StringList = new List<string>();
                AES += "inE(";
                foreach (var x in Parameters)
                    AES += "\'" + x + "\'";
                AES += ").";
            }
            else
                AES += "inE().";
            if (HoldMark == true) held = this;

            return new GraphTraversal(this);
        }

        public GraphTraversal inV()
        {
            if (HoldMark == true) held = this;

            return new GraphTraversal(this, AppendExecutableString + "inV().");

        }

        public GraphTraversal outV()
        {
            if (HoldMark == true) held = this;

            return new GraphTraversal(this, AppendExecutableString + "outE().");

        }
        public GraphTraversal As(string alias)
        {
            if (HoldMark == true) held = this;

            return new GraphTraversal(this, AppendExecutableString + "as(\'" + alias + "\').");

        }

        public GraphTraversal select(params string[] Parameters)
        {
            string AES = AppendExecutableString;
            if (Parameters == null)
                AES += "select().";
            else
                AES += "select(\'"+ string.Join("\',\'",Parameters)+"\').";
            if (HoldMark == true) held = this;
            return new GraphTraversal(this,AES);

        }

        public GraphTraversal addV(params string[] Parameters)
        {

            //GraphViewGremlinParser parser = new GraphViewGremlinParser();
            //parser.Parse(CutTail(AppendExecutableString + "addV(\'" + string.Join("\',\'", Parameters) + "\').")).Generate(connection).Next();

            if (HoldMark == true) held = this;

            return new GraphTraversal(this, AppendExecutableString + "addV(\'" + string.Join("\',\'", Parameters) + "\').");

        }

        public GraphTraversal addV(List<string> Parameters)
        {

            //GraphViewGremlinParser parser = new GraphViewGremlinParser();
            //parser.Parse(CutTail(AppendExecutableString + "addV(\'" + string.Join("\',\'", Parameters) + "\').")).Generate(connection).Next();

            if (HoldMark == true) held = this;

            return new GraphTraversal(this, AppendExecutableString + "addV(\'" + string.Join("\',\'", Parameters) + "\').");

        }

        public GraphTraversal addOutE(params string[] Parameters)
        {
            //GraphViewGremlinParser parser = new GraphViewGremlinParser();
            //parser.Parse(CutTail(AppendExecutableString + "addOutE(\'" + string.Join("\',\'", Parameters) + "\').")).Generate(connection).Next();

            if (HoldMark == true) held = this;


            return new GraphTraversal(this, AppendExecutableString + "addOutE(\'" + string.Join("\',\'", Parameters) + "\').");

        }

        public GraphTraversal addInE(params string[] Parameters)
        {
            //GraphViewGremlinParser parser = new GraphViewGremlinParser();
            //parser.Parse(CutTail(AppendExecutableString + "addInE(\'" + string.Join("\',\'", Parameters) + "\').")).Generate(connection).Next();

            if (HoldMark == true) held = this;

            return new GraphTraversal(this, AppendExecutableString + "addInE(\'" + string.Join("\',\'", Parameters) + "\').");
        }

        public GraphTraversal values(string name)
        {
            if (HoldMark == true) held = this;

            return new GraphTraversal(this, AppendExecutableString + "values(\'" + name + "\').");

        }

        public GraphTraversal where(Tuple<string, GraphViewGremlinParser.Keywords> ComparisonFunc)
        {

            string AES = AppendExecutableString;
            if (HoldMark == true) held = this;

            if (ComparisonFunc.Item2 == GraphViewGremlinParser.Keywords.eq)
                AES += "where(eq(\'" + ComparisonFunc.Item1 + "\')).";
            if (ComparisonFunc.Item2 == GraphViewGremlinParser.Keywords.neq)
                AES += "where(neq(\'" + ComparisonFunc.Item1 + "\')).";

            return new GraphTraversal(this,AES);
        }

        public GraphTraversal match(params GraphTraversal[] pipes)
        {
            string AES = AppendExecutableString;
            List<string> StringList = new List<string>();
            foreach (var x in pipes) StringList.Add(x.AppendExecutableString);
            AES += "match(\'" + String.Join(",", StringList) + ").";
            return new GraphTraversal(this,AES);
        }

        public GraphTraversal aggregate(string name)
        {
            if (HoldMark == true) held = this;

            return new GraphTraversal(this);
        }

        public GraphTraversal and(params GraphTraversal[] pipes)
        {

            List<string> PipeString = new List<string>();
            foreach(var x in pipes) PipeString.Add(x.AppendExecutableString);
            return new GraphTraversal(this, AppendExecutableString + "and(" + String.Join(",", PipeString) + ").");
        }

        public GraphTraversal or(params GraphTraversal[] pipes)
        {
            List<string> PipeString = new List<string>();
            foreach (var x in pipes) PipeString.Add(x.AppendExecutableString);
            return new GraphTraversal(this, AppendExecutableString + "or(" + String.Join(",", PipeString) + ").");
        }

        public GraphTraversal drop()
        {
            if (HoldMark == true) held = this;

            return new GraphTraversal(this, AppendExecutableString + "drop().");
        }

        public GraphTraversal Is(Tuple<string, GraphViewGremlinParser.Keywords> ComparisonFunc)
        {
            string AES = AppendExecutableString;
            if (HoldMark == true) held = this;

            if (ComparisonFunc.Item2 == GraphViewGremlinParser.Keywords.eq)
                AES += "is(eq(\'" + ComparisonFunc.Item1 + "\')).";
            if (ComparisonFunc.Item2 == GraphViewGremlinParser.Keywords.neq)
                AES += "is(neq(\'" + ComparisonFunc.Item1 + "\')).";

            return new GraphTraversal(this);
        }

        public GraphTraversal Limit(int i)
        {
            if (HoldMark == true) held = this;

            return new GraphTraversal(this);
        }

        public GraphTraversal repeat(GraphTraversal pipe)
        {

            return new GraphTraversal(this, AppendExecutableString + "repeat(" + CutTail(pipe.AppendExecutableString) + ").");
        }

        public GraphTraversal times(int i)
        {
            return new GraphTraversal(this, AppendExecutableString + "times(" + i + ").");
        }

        public GraphTraversal choose(GraphTraversal pipe)
        {
            return new GraphTraversal(this, AppendExecutableString + "choose(" + CutTail(pipe.AppendExecutableString) + ").");
        }

        public GraphTraversal option(string name, GraphTraversal pipe)
        {
            return new GraphTraversal(this, AppendExecutableString + "option(\'" + name + "\'" + CutTail(pipe.AppendExecutableString) + ").");
        }

        public GraphTraversal coalesce(params GraphTraversal[] pipes)
        {
            List<string> StringList = new List<string>();
            foreach(var x in pipes) StringList.Add(CutTail(x.AppendExecutableString));
            return new GraphTraversal(this, AppendExecutableString + "coalesce(" + String.Join(",", StringList) + ").");
        }

        public GraphTraversal addE(params string[] Parameters)
        {
            if (HoldMark == true) held = this;

            return new GraphTraversal(this, AppendExecutableString + "addE(\'" + string.Join("\',\'", Parameters) + "\').");
        }

        public GraphTraversal from(GraphTraversal OtherSource)
        {
            GraphTraversal NewTraversal = new GraphTraversal(this);
            if (OtherSource != null)
            {
                NewTraversal.AddEdgeOtherSource = OtherSource;
                NewTraversal.dir = direction.In;
            }
            return NewTraversal;
        }

        public GraphTraversal to(GraphTraversal OtherSource)
        {
            GraphTraversal NewTraversal = new GraphTraversal(this);
            if (OtherSource != null)
            {
                NewTraversal.AddEdgeOtherSource = OtherSource;
                NewTraversal.dir = direction.Out;
            }
            return NewTraversal;
        }

        public GraphTraversal order()
        {
            return new GraphTraversal(this, AppendExecutableString + "order().");
        }

        public GraphTraversal by(string bywhat,string order ="")
        {
            if (order == "" && bywhat =="incr")
                return new GraphTraversal(this, AppendExecutableString + "by(incr).");
            if (order == "" && bywhat == "decr")
                return new GraphTraversal(this, AppendExecutableString + "by(decr).");
            return new GraphTraversal(this, AppendExecutableString + "by(\'" + bywhat + "\', " + order + ").");
        }

        public GraphTraversal max()
        {
            return new GraphTraversal(this, AppendExecutableString + "max().");
        }
        public GraphTraversal count()
        {
            return new GraphTraversal(this, AppendExecutableString + "count().");
        }
        public GraphTraversal min()
        {
            return new GraphTraversal(this, AppendExecutableString + "min().");
        }
        public GraphTraversal mean()
        {
            return new GraphTraversal(this, AppendExecutableString + "mean().");
        }
        internal string CutTail(string some)
        {
            if (some.Length < 1) return null;
            return some.Substring(0, some.Length - 1);
        }
    }

}