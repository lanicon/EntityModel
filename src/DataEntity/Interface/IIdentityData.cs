// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     ��ʾ��Ψһ���ֱ�ʶ������
    /// </summary>
    public interface IIdentityData
    {
        /// <summary>
        ///     ���ֱ�ʶ
        /// </summary>
        /// <value>int</value>
        int Id { get; set; }
    }
    /// <summary>
    ///     ��ʾ�б��������
    /// </summary>
    public interface ITitle
    {
        /// <summary>
        ///     ����
        /// </summary>
        /// <value>int</value>
        string Title { get; }
    }
    /// <summary>
    ///     ��ʾ��Ψһ���ֱ�ʶ������
    /// </summary>
    public interface IUnionUniqueEntity
    {
    }
    
}