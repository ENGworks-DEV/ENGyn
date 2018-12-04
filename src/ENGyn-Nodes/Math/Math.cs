using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using NCalc;
using TUM.CMS.VplControl.Core;
using TUM.CMS.VplControl.Utilities;


namespace Nodes.Math
{
    public class ExpressionNode2 : Node
    {
        private Expression expression;

        public ExpressionNode2(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddOutputPortToNode("Result", typeof(object));

            var textBox = new TextBox { MinWidth = 120, MaxWidth = 300, IsHitTestVisible = false };
            textBox.TextChanged += textBox_TextChanged;
            textBox.KeyUp += textBox_KeyUp;

            AddControlToNode(textBox);

            MouseEnter += ExpressionNode_MouseEnter;
            MouseLeave += ExpressionNode_MouseLeave;
        }

        private void textBox_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void ExpressionNode_MouseLeave(object sender, MouseEventArgs e)
        {
            Border.Focusable = true;
            Border.Focus();
            Border.Focusable = false;
        }

        private void ExpressionNode_MouseEnter(object sender, MouseEventArgs e)
        {
            ControlElements[0].Focus();
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;

            if (textBox != null)
            {
                if (textBox.Text != "")
                {
                    expression = new Expression(textBox.Text);

                    var paras = GetParametersInExpression(textBox.Text).Distinct().ToList();

                    if (paras.Any())
                    {
                        RemoveAllInputPortsFromNode(paras);

                        var filteredParas = paras.Where(parameter => InputPorts.All(p => p.Name != parameter)).ToList();

                        foreach (var parameter in filteredParas)
                            AddInputPortToNode(parameter, typeof(object));
                    }
                }
                else
                {
                    expression = null;
                    RemoveAllInputPortsFromNode();
                }
            }

            //Calculate();
        }

        public List<string> GetParametersInExpression(string formula)
        {
            try
            {
                var expr = Expression.Compile(formula, false);

                var visitor = new ParameterExtractionVisitor();
                expr.Accept(visitor);

                return visitor.Parameters;
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }

        private static List<object> StrechArgumentLists(List<Port> ports, List<Tuple<int, object>> values)
        {
            List<object> result = new List<object>();
            //Fill values list
            foreach (var arg in ports)
            {

                System.Collections.IList argList = null;
                if (ENGyn.Nodes.MainTools.IsList(arg.Data))
                {
                    argList = (System.Collections.IList)arg.Data;
                }
                //Check if argument is a list and include it on a Tuple with its count number
                var argResult = argList != null ? new Tuple<int, object>(argList.Count, argList) : new Tuple<int, object>(1, arg.Data);
                //if args are list, count the number of object to set how many times it should run
                values.Add(argResult);

            }
            //Get highest value == longest list count
            int Highest = values.OrderBy(x => x.Item1).Reverse().ElementAt(0).Item1;



            for (int i = 0; i < Highest; i++)
            {
                var ActualArgList = new List<object>();
                foreach (var item in values)
                {
                    object currentArg = null;
                    if (item.Item1 > 1 || ENGyn.Nodes.MainTools.IsList(item.Item2))
                    {
                        var tempArgList = (List<object>)item.Item2;
                        //get latest object

                        var currentArgListLenght = tempArgList.Count - 1;
                        currentArg = currentArgListLenght <= i ? tempArgList[currentArgListLenght] : tempArgList[i];
                        ActualArgList.Add(currentArg);
                    }
                    else
                    {
                        currentArg = item.Item2;
                        ActualArgList.Add(currentArg);
                    }
                }
                List<object> iterator =new List<object> ();

                for (int ii = 0; ii < ports.Count; ii++)
                {
                    var t = new Tuple<string, object>(ports[ii].Name, ActualArgList[ii]);
                    iterator.Add(t);
                }

                result.Add(iterator);
            }
            return result;
        }

        public override void Calculate()
        {
            if (expression != null)
            {
                List<Tuple<int, object>> values = new List<Tuple<int, object>> ();
                var result = StrechArgumentLists(InputPorts, values);
                var output = new List<object>();
                foreach (List<object> port in result)
                {
                    foreach (var item in (List<object>)port)
                    {
                        var t = item as Tuple<string, object>;
                        expression.Parameters[t.Item1] = t.Item2;
                    }

                    try
                    {
                        output.Add(expression.Evaluate());
                    }
                    catch (Exception ex)
                    {
                        output.Add(null);
                    }
                }

                OutputPorts[0].Data = output;
            }
            else
                OutputPorts[0].Data = null;
        }

        public override void SerializeNetwork(XmlWriter xmlWriter)
        {
            base.SerializeNetwork(xmlWriter);

            var textBox = ControlElements[0] as TextBox;
            if (textBox == null) return;

            xmlWriter.WriteStartAttribute("Formula");
            xmlWriter.WriteValue(textBox.Text);
            xmlWriter.WriteEndAttribute();
        }

        public override void DeserializeNetwork(XmlReader xmlReader)
        {
            base.DeserializeNetwork(xmlReader);

            var textBox = ControlElements[0] as TextBox;
            if (textBox == null) return;

            textBox.Text = xmlReader.GetAttribute("Formula");
        }

        public override Node Clone()
        {
            return new ExpressionNode2(HostCanvas)
            {
                Top = Top,
                Left = Left
            };
        }
    }
}