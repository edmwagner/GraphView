﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphView.GremlinTranslationOps.map
{
    internal class GremlinInVOp: GremlinTranslationOperator
    {
        public GremlinInVOp() { }

        public override GremlinToSqlContext GetContext()
        {
            GremlinToSqlContext inputContext = GetInputContext();

            //GremlinUtil.CheckIsGremlinEdgeVariable(inputContext.CurrVariable);

            GremlinVariable inVariable = null;

            if (inputContext.CurrVariable is GremlinAddEVariable)
            {
                inVariable = (inputContext.CurrVariable as GremlinAddEVariable).ToVariable;
            }
            else
            {
                inVariable = inputContext.Paths.Find(p => p.Item2 == inputContext.CurrVariable).Item3;
            }
            
            inputContext.SetCurrVariable(inVariable);
            inputContext.SetDefaultProjection(inVariable);

            return inputContext;
        }
    }
}
