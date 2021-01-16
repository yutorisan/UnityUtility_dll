using System;
using System.Security.AccessControl;
using System.Threading;

namespace UnityUtility
{
    /// <summary>
    /// 角度
    /// </summary>
    public readonly struct Angle : IEquatable<Angle>, IComparable<Angle>
    {
        /// <summary>
        /// 正規化していない角度の累積値
        /// </summary>
        private readonly float m_totalDegree;

        /// <summary>
        /// 角度を度数法で指定して、新規インスタンスを作成します。
        /// </summary>
        /// <param name="angle">度数法の角度</param>
        private Angle(float angle) => m_totalDegree = angle;
        /// <summary>
        /// 周回数と角度を指定して、新規インスタンスを作成します。
        /// </summary>
        /// <param name="lap">周回数</param>
        /// <param name="angle">度数法の角度</param>
        private Angle(int lap, float angle) => m_totalDegree = checked(360 * lap + angle);

        /// <summary>
        /// 度数法の値を使用して新規インスタンスを取得します。
        /// </summary>
        /// <param name="degree">度数法の角度(°)</param>
        /// <returns></returns>
        public static Angle FromDegree(float degree) => new Angle(degree);
        /// <summary>
        /// 周回数と角度を指定して、新規インスタンスを取得します。
        /// </summary>
        /// <param name="lap">周回数</param>
        /// <param name="degree">度数法の角度(°)</param>
        /// <returns></returns>
        public static Angle FromDegree(int lap, float degree) => new Angle(lap, degree);
        /// <summary>
        /// 弧度法の値を使用して新規インスタンスを取得します。
        /// </summary>
        /// <param name="radian">弧度法の角度(rad)</param>
        /// <returns></returns>
        public static Angle FromRadian(float radian) => new Angle(radian.ToDegree());
        /// <summary>
        /// 周回数と角度を指定して、新規インスタンスを取得します。
        /// </summary>
        /// <param name="lap">周回数</param>
        /// <param name="radian">弧度法の角度(rad)</param>
        /// <returns></returns>
        public static Angle FromRadian(int lap, float radian) => new Angle(lap, radian.ToDegree());
        /// <summary>
        /// 角度0°の新規インスタンスを取得します。
        /// </summary>
        public static Angle Zero => new Angle(0);
        /// <summary>
        /// 角度360°の新規インスタンスを取得します。
        /// </summary>
        public static Angle Round => new Angle(360);

        public bool Equals(Angle other) => m_totalDegree == other.m_totalDegree;

        public override int GetHashCode() => -1748791360 + m_totalDegree.GetHashCode();

        public override string ToString() => $"{Lap}x + {m_totalDegree - Lap * 360}°";

        public override bool Equals(object obj)
        {
            if (obj is Angle angle) return Equals(angle);
            else return false;
        }

        public int CompareTo(Angle other) => m_totalDegree.CompareTo(other.m_totalDegree);

        /// <summary>
        /// 正規化された角度(-180° < degree <= 180°)を取得します。
        /// </summary>
        /// <returns></returns>
        public Angle Normalize() => new Angle(NormalizedDegree);
        /// <summary>
        /// 正の値で正規化された角度(0° <= degree < 360°)を取得します。
        /// </summary>
        /// <returns></returns>
        public Angle PositiveNormalize() => new Angle(PositiveNormalizedDegree);
        /// <summary>
        /// 方向を反転させた角度を取得します。
        /// 例：90°→-270°, -450°→630°
        /// </summary>
        /// <returns></returns>
        public Angle Reverse()
        {
            //ゼロならゼロ
            if (this == Zero) return Zero;
            //真円の場合は真逆にする
            if (IsTrueCircle) return new Angle(-Lap, 0);
            if (IsCircled)
            { //1周以上している
                if (IsPositive)
                { //360~
                    return new Angle(-Lap, NormalizedDegree - 360);
                }
                else
                { //~-360
                    return new Angle(-Lap, NormalizedDegree + 360);
                }
            }
            else
            { //1周していない
                if (IsPositive)
                { //0~360
                    return new Angle(m_totalDegree - 360);
                }
                else
                { //-360~0
                    return new Angle(m_totalDegree + 360);
                }
            }
        }

        /// <summary>
        /// 正規化していない角度値を取得します。
        /// </summary>
        public float TotalDegree => m_totalDegree;
        /// <summary>
        /// 正規化していない角度値をラジアンで取得します。
        /// </summary>
        public float TotalRadian => TotalDegree.ToRadian();
        /// <summary>
        /// 正規化された角度値(-180 < angle <= 180)を取得します。
        /// </summary>
        public float NormalizedDegree
        {
            get
            {
                float tmpNormalized = m_totalDegree - (Lap * 360);
                if (tmpNormalized > 180) return tmpNormalized - 360;
                if (tmpNormalized <= -180) return tmpNormalized + 360;
                return tmpNormalized;
            }
        }

        /// <summary>
        /// 正規化された角度値をラジアン(-π < rad < π)で取得します。
        /// </summary>
        public float NormalizedRadian => NormalizedDegree.ToRadian();
        /// <summary>
        /// 正規化された角度値(0 <= angle < 360)を取得します。
        /// </summary>
        public float PositiveNormalizedDegree
        {
            get
            {
                var normalized = NormalizedDegree;
                return normalized >= 0 ? normalized : normalized + 360;
            }
        }

        /// <summary>
        /// 正規化された角度値をラジアン(0 <= rad < 2π)で取得します。
        /// </summary>
        public float PositiveNormalizedRadian => PositiveNormalizedDegree.ToRadian();
        /// <summary>
        /// 角度が何周しているかを取得します。
        /// 例：370°→1周, -1085°→-3周
        /// </summary>
        public int Lap => ((int)m_totalDegree) / 360;
        /// <summary>
        /// 1周以上しているかどうか(360°以上、もしくは-360°以下かどうか)を取得します。
        /// </summary>
        public bool IsCircled => Lap != 0;
        /// <summary>
        /// 360の倍数の角度であるかどうかを取得します。
        /// </summary>
        public bool IsTrueCircle => IsCircled && m_totalDegree % 360 == 0;
        /// <summary>
        /// 正の角度かどうかを取得します。
        /// </summary>
        public bool IsPositive => m_totalDegree >= 0;


        public static Angle operator +(Angle left, Angle right) => new Angle(checked(left.m_totalDegree + right.m_totalDegree));
        public static Angle operator -(Angle left, Angle right) => new Angle(checked(left.m_totalDegree - right.m_totalDegree));
        public static Angle operator *(Angle left, float right) => new Angle(checked(left.m_totalDegree * right));
        public static Angle operator /(Angle left, float right) => new Angle(checked(left.m_totalDegree / right));
        public static bool operator ==(Angle left, Angle right) => left.m_totalDegree == right.m_totalDegree;
        public static bool operator !=(Angle left, Angle right) => left.m_totalDegree != right.m_totalDegree;
        public static bool operator >(Angle left, Angle right) => left.m_totalDegree > right.m_totalDegree;
        public static bool operator <(Angle left, Angle right) => left.m_totalDegree < right.m_totalDegree;
        public static bool operator >=(Angle left, Angle right) => left.m_totalDegree >= right.m_totalDegree;
        public static bool operator <=(Angle left, Angle right) => left.m_totalDegree <= right.m_totalDegree;
    }
}
