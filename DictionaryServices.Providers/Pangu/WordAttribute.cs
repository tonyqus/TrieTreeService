﻿/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Text;
using BluePrint.SegmentFramework;

namespace BluePrint.Dictionary.Providers
{
    public class WordAttribute : IComparable<WordAttribute>, IDataNode
    {
        /// <summary>
        /// Word
        /// </summary>
        public String Word { get; set; }

        /// <summary>
        /// Part of speech
        /// </summary>
        public POSType POS { get; set; }

        /// <summary>
        /// Frequency for this word
        /// </summary>
        public double Frequency { get; set; }

        public WordAttribute()
        {

        }

        public WordAttribute(string word, POSType pos, double frequency)
        {
            this.Word = word;
            this.POS = pos;
            this.Frequency = frequency;
        }

        public override string ToString()
        {
            return Word;
        }


        #region IComparable<WordAttribute> Members

        public int CompareTo(WordAttribute other)
        {
            return this.Word.CompareTo(other.Word);
        }

        #endregion
    }

}