﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphView.GremlinTranslationOps
{
    internal class GremlinVOp: GremlinTranslationOperator
    {
        public List<object> VertexIdsOrElements;
        public GremlinVOp(params object[] vertexIdsOrElements)
        {
            VertexIdsOrElements = new List<object>();
            foreach (var vertexIdsOrElement in vertexIdsOrElements)
            {
                VertexIdsOrElements.Add(vertexIdsOrElement);
            }
        }

        public GremlinVOp(List<object> vertexIdsOrElements)
        {
            VertexIdsOrElements = vertexIdsOrElements;
        }
        public override GremlinToSqlContext GetContext()
        {
            GremlinToSqlContext inputContext = GetInputContext();
            GremlinVertexVariable newVertexVar = new GremlinVertexVariable();

            foreach (var id in VertexIdsOrElements)
            {
                if (id is int)
                {
                    WScalarExpression key = GremlinUtil.GetColumnReferenceExpression(newVertexVar.VariableName, "id");
                    WBooleanComparisonExpression booleanExpr = GremlinUtil.GetBooleanComparisonExpr(key, id);
                    inputContext.AddPredicate(booleanExpr);
                }
            }

            inputContext.AddNewVariable(newVertexVar);
            inputContext.SetCurrVariable(newVertexVar);
            inputContext.SetDefaultProjection(newVertexVar);
            return inputContext;
        }
    }
}