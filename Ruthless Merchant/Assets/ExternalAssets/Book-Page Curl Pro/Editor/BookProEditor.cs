using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditorInternal;
using System;

[CustomEditor(typeof(BookPro))]
public class BookProEditor : Editor
{
    ReorderableList list;
    Texture tex;
    private void OnEnable()
    {
        tex = AssetDatabase.LoadAssetAtPath("Assets\\Book-Page Curl\\Editor\\replace.png", typeof(Texture)) as Texture;
        if (tex == null)
        {
            tex = Texture2D.blackTexture;
        }
        list = new ReorderableList(serializedObject,
                serializedObject.FindProperty("papers"),
                true, true, true, true);
        list.elementHeight = 75;
        list.drawElementCallback = DrawElement;
        list.drawHeaderCallback = drawHeader;
        list.onAddCallback = addElement;

        list.onCanRemoveCallback = canremove;

        list.onRemoveCallback = (ReorderableList l) =>
        {
            if (EditorUtility.DisplayDialog("Warning!",
                "Are you sure you want to delete this Paper?\r\nThe paper pages (front and back) will be deleted from the scene", "Yes", "No"))
            {
                BookPro book = target as BookPro;
                if (book.EndFlippingPaper == book.papers.Length - 1)
                    book.EndFlippingPaper--;
                OnInspectorGUI();
                Paper paper = book.papers[l.index];

                book.LeftPageShadow.gameObject.SetActive(false);
                book.LeftPageShadow.transform.SetParent(book.transform);

                book.RightPageShadow.gameObject.SetActive(false);
                book.RightPageShadow.transform.SetParent(book.transform);

                Undo.DestroyObjectImmediate(paper.Back);
                Undo.DestroyObjectImmediate(paper.Front);
                ReorderableList.defaultBehaviours.DoRemoveButton(l);
                EditorUtility.SetDirty(book);
            }
        };
    }

    private bool canremove(ReorderableList list)
    {
        if (list.count == 1)
            return false;
        return true;
    }

    private void addElement(ReorderableList list)
    {
        BookPro book = target as BookPro;

        if (book.EndFlippingPaper == book.papers.Length - 1)
        {
            book.EndFlippingPaper = book.papers.Length;
            OnInspectorGUI();
        }

        list.serializedProperty.arraySize++;
        var lastElement = list.serializedProperty.GetArrayElementAtIndex(list.count - 1);


        GameObject rightPage = Instantiate(book.RightPageTransform.gameObject) as GameObject;
        rightPage.transform.SetParent(book.transform, true);
        rightPage.GetComponent<RectTransform>().sizeDelta = book.RightPageTransform.GetComponent<RectTransform>().sizeDelta;
        rightPage.GetComponent<RectTransform>().pivot = book.RightPageTransform.GetComponent<RectTransform>().pivot;
        rightPage.GetComponent<RectTransform>().anchoredPosition = book.RightPageTransform.GetComponent<RectTransform>().anchoredPosition;
        rightPage.GetComponent<RectTransform>().localScale = book.RightPageTransform.GetComponent<RectTransform>().localScale;
        rightPage.name = "Page" + ((list.serializedProperty.arraySize - 1) * 2);
        rightPage.AddComponent<Image>();
        rightPage.AddComponent<Mask>().showMaskGraphic = true;
        rightPage.AddComponent<CanvasGroup>();
        lastElement.FindPropertyRelative("Front").objectReferenceInstanceIDValue = rightPage.GetInstanceID();


        GameObject leftPage = Instantiate(book.LeftPageTransform.gameObject) as GameObject;
        leftPage.transform.SetParent(book.transform, true);
        leftPage.GetComponent<RectTransform>().sizeDelta = book.LeftPageTransform.GetComponent<RectTransform>().sizeDelta;
        leftPage.GetComponent<RectTransform>().pivot = book.LeftPageTransform.GetComponent<RectTransform>().pivot;
        leftPage.GetComponent<RectTransform>().anchoredPosition = book.LeftPageTransform.GetComponent<RectTransform>().anchoredPosition;
        leftPage.GetComponent<RectTransform>().localScale = book.LeftPageTransform.GetComponent<RectTransform>().localScale;
        leftPage.name = "Page" + ((list.serializedProperty.arraySize - 1) * 2 + 1);
        leftPage.AddComponent<Image>();
        leftPage.AddComponent<Mask>().showMaskGraphic = true;
        leftPage.AddComponent<CanvasGroup>();
        lastElement.FindPropertyRelative("Back").objectReferenceInstanceIDValue = leftPage.GetInstanceID();
        list.index = list.count - 1;


        Undo.RegisterCreatedObjectUndo(leftPage, "");
        Undo.RegisterCreatedObjectUndo(rightPage, "");
    }

    private void drawHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Papers");
    }

    private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        BookPro book = target as BookPro;

        var serialzedpaper = list.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;

        GUIStyle style = new GUIStyle();

        EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, rect.height - 6), new Color(0.8f, 0.8f, 0.8f));
        EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), "Paper#" + index);

        if (index == book.CurrentPaper)
            EditorGUI.DrawRect(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, 140, EditorGUIUtility.singleLineHeight), new Color(1, 0.3f, 0.3f));
        EditorGUI.LabelField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, 40, EditorGUIUtility.singleLineHeight), "Front:");
        EditorGUI.PropertyField(
            new Rect(rect.x + 40, rect.y + EditorGUIUtility.singleLineHeight, 100, EditorGUIUtility.singleLineHeight), serialzedpaper.FindPropertyRelative("Front"), GUIContent.none);

        if (index == book.CurrentPaper - 1)
            EditorGUI.DrawRect(new Rect(rect.x, rect.y + 3 * EditorGUIUtility.singleLineHeight, 140, EditorGUIUtility.singleLineHeight), new Color(1, 0.3f, 0.3f));
        EditorGUI.LabelField(new Rect(rect.x, rect.y + 3 * EditorGUIUtility.singleLineHeight, 35, EditorGUIUtility.singleLineHeight), "Back:");
        EditorGUI.PropertyField(
        new Rect(rect.x + 40, rect.y + 3 * EditorGUIUtility.singleLineHeight, 100, EditorGUIUtility.singleLineHeight), serialzedpaper.FindPropertyRelative("Back"), GUIContent.none);


        style.padding = new RectOffset(2, 2, 2, 2);
        if (GUI.Button(new Rect(rect.x + 70, rect.y + 2 * EditorGUIUtility.singleLineHeight, 20, EditorGUIUtility.singleLineHeight), tex, GUIStyle.none))
        {
            Debug.Log("Clicked at index:" + index);
            Paper paper = book.papers[index];
            GameObject temp = paper.Back;
            paper.Back = paper.Front;
            paper.Front = temp;
        }
        if (GUI.Button(new Rect(rect.x + 150, rect.y + EditorGUIUtility.singleLineHeight, 80, (int)(1.5 * EditorGUIUtility.singleLineHeight)), "Show"))
        {
            Paper paper = book.papers[index];
            BookUtility.ShowPage(paper.Back);
            paper.Back.transform.SetAsLastSibling();
            BookUtility.ShowPage(paper.Front);
            paper.Front.transform.SetAsLastSibling();

            book.LeftPageShadow.gameObject.SetActive(false);
            book.LeftPageShadow.transform.SetParent(book.transform);

            book.RightPageShadow.gameObject.SetActive(false);
            book.RightPageShadow.transform.SetParent(book.transform);
        }
        if (GUI.Button(new Rect(rect.x + 150, rect.y + (int)(2.5 * EditorGUIUtility.singleLineHeight), 80, (int)(1.5 * EditorGUIUtility.singleLineHeight)), "Set Current"))
        {
            book.CurrentPaper = index;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        DrawNavigationPanel();
        DrawFlippingPapersRange();
        if (GUILayout.Button("Update Pages Order"))
        {
            (target as BookPro).UpdatePages();
        }
        if (GUILayout.Button("Update Pages Names"))
        {
            if (EditorUtility.DisplayDialog("Warning!",
                "All Pages will be renamed according to its order", "Ok", "Cancel"))
            {
                BookPro book = target as BookPro;
                for (int i = 0; i < book.papers.Length; i++)
                {
                    book.papers[i].Front.name = "Page" + (i * 2);
                    book.papers[i].Back.name = "Page" + (i * 2 + 1);
                }

            }
        }
    }

    private void DrawFlippingPapersRange()
    {
        BookPro book = target as BookPro;
#if UNITY_5_3_OR_NEWER
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
#else
        EditorGUILayout.BeginVertical(EditorStyles.textArea);
#endif


        EditorGUILayout.LabelField("Flipping Range", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("First Flippable Paper: " + "Paper#" + book.StartFlippingPaper);
        EditorGUILayout.LabelField("Last Flippable Paper: " + "Paper#" + book.EndFlippingPaper);
        float start = book.StartFlippingPaper;
        float end = book.EndFlippingPaper;
        EditorGUILayout.MinMaxSlider(ref start, ref end, 0, book.papers.Length - 1);
        book.StartFlippingPaper = Mathf.RoundToInt(start);
        book.EndFlippingPaper = Mathf.RoundToInt(end);
        EditorGUILayout.EndVertical();
    }

    private void DrawNavigationPanel()
    {

        BookPro book = target as BookPro;
#if UNITY_5_3_OR_NEWER
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
#else
        EditorGUILayout.BeginHorizontal(EditorStyles.textArea);
#endif

        GUILayout.Label(new GUIContent("Current Paper", "represent current paper on the right side of the book. if you want to show the back page of the last paper you may set this value with the index of last paper + 1"));
        if (GUILayout.Button("|<"))
        {
            book.CurrentPaper = 0;
        }
        if (GUILayout.Button("<"))
        {
            book.CurrentPaper--;
        }
        book.CurrentPaper = EditorGUILayout.IntField(book.CurrentPaper, GUILayout.Width(30));
        EditorUtility.SetDirty(book);
        Undo.RecordObject(book, "CurrentPageChange");
        if (GUILayout.Button(">"))
        {
            book.CurrentPaper++;
        }
        if (GUILayout.Button(">|"))
        {
            book.CurrentPaper = book.papers.Length;
        }
        EditorGUILayout.EndHorizontal();

    }

}
