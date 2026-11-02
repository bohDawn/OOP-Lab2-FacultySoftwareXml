<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<xsl:output method="html" encoding="utf-8"/>

<xsl:template match="FacultySoftwareCatalog">
  <HTML>
    <BODY>
      <p>
        <H2>Програмне забезпечення на мережі факультету</H2>
      </p>
      <TABLE BORDER="2">
        <TR>
          <TD><b>Назва</b></TD>
          <TD><b>Вид</b></TD>
          <TD><b>Версія</b></TD>
          <TD><b>Архітектура</b></TD>
          <TD><b>Рік випуску</b></TD>
          <TD><b>Права адміністратора</b></TD>
          <TD><b>Анотація</b></TD>
          <TD><b>Автор</b></TD>
          <TD><b>Факультет</b></TD>
          <TD><b>Кафедра</b></TD>
          <TD><b>Ліцензія</b></TD>
          <TD><b>Умови використання</b></TD>
          <TD><b>Дистрибутив</b></TD>
        </TR>
        <xsl:apply-templates select="SoftwareItem"/>
      </TABLE>
    </BODY>
  </HTML>
</xsl:template>

<xsl:template match="SoftwareItem">
  <TR>
    <TD><b><xsl:value-of select="@Name"/></b></TD>
    <TD><xsl:value-of select="@Type"/></TD>
    <TD><xsl:value-of select="@Version"/></TD>
    <TD><xsl:value-of select="@Architecture"/></TD>
    <TD><xsl:value-of select="@ReleaseYear"/></TD>
    <TD><xsl:value-of select="@RequiresAdmin"/></TD>
    <TD><xsl:value-of select="Annotation"/></TD>
    <TD><xsl:value-of select="Author/@FullName"/></TD>
    <TD><xsl:value-of select="Author/@Faculty"/></TD>
    <TD><xsl:value-of select="Author/@Chair"/></TD>
    <TD><xsl:value-of select="UsageTerms/@License"/></TD>
    <TD><xsl:value-of select="UsageTerms"/></TD>
    <TD>
      <xsl:for-each select="Distribution">
        <p><xsl:value-of select="@Medium"/></p>
        <p><xsl:value-of select="@Path"/></p>
      </xsl:for-each>
    </TD>
  </TR>
</xsl:template>

</xsl:stylesheet>
