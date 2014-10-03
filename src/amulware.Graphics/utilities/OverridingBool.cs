using System;

namespace amulware.Graphics
{
    public struct OverridingBool
    {
        private readonly bool @override;
        private readonly bool value;

        public OverridingBool(bool overrideDefault, bool value)
        {
            this.@override = overrideDefault;
            this.value = value;
        }

        public bool Overrides { get { return this.@override; } }

        public bool Value
        {
            get
            {
                if (!this.@override)
                    throw new Exception("Overriding bool has no value!");
                return this.value;
            }
        }

        public bool OrDefault(bool defaultValue)
        {
            return this.@override ? this.value : defaultValue;
        }

        public static implicit operator OverridingBool(bool b)
        {
            return new OverridingBool(true, b);
        }
    }
}
